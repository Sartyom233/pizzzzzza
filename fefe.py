import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
import numpy as np

class MatrixVectorCalculator:
    def __init__(self, root):
        self.root = root
        self.root.title("Матрично-векторный калькулятор")
        self.root.geometry("900x700")
        self.notebook = ttk.Notebook(root)
        self.notebook.pack(fill='both', expand=True)
        self.create_matrix_tab()
        self.create_slau_tab()
        self.create_vector_tab()
        self.output = scrolledtext.ScrolledText(root, height=10, state='normal')
        self.output.pack(fill='x', padx=5, pady=5)

    def log(self, text):
        self.output.insert(tk.END, text + "\n")
        self.output.see(tk.END)
        self.root.update()

    def clear_output(self):
        self.output.delete(1.0, tk.END)

    def create_matrix_tab(self):
        tab = ttk.Frame(self.notebook)
        self.notebook.add(tab, text="Матрицы")
        tk.Label(tab, text="Строки:").grid(row=0, column=0)
        self.m_rows = tk.Entry(tab, width=5)
        self.m_rows.grid(row=0, column=1)
        tk.Label(tab, text="Столбцы:").grid(row=0, column=2)
        self.m_cols = tk.Entry(tab, width=5)
        self.m_cols.grid(row=0, column=3)
        tk.Button(tab, text="Создать матрицы", command=self.create_matrix_inputs).grid(row=0, column=4)
        self.matrix_frame = tk.Frame(tab)
        self.matrix_frame.grid(row=1, column=0, columnspan=5, pady=10)
        self.matrix_entries = []
        self.matrix2_entries = []
        tk.Label(tab, text="Операция:").grid(row=2, column=0)
        self.m_op = ttk.Combobox(tab, values=["Сложение", "Вычитание", "Умножение на число", "Умножение матриц", "Транспонирование", "Определитель", "Ранг"])
        self.m_op.grid(row=2, column=1)
        self.m_scalar = tk.Entry(tab, width=10)
        self.m_scalar.grid(row=2, column=2)
        self.m_scalar.insert(0, "2")
        tk.Button(tab, text="Вычислить", command=self.do_matrix_op).grid(row=2, column=3)

    def create_matrix_inputs(self):
        for widget in self.matrix_frame.winfo_children():
            widget.destroy()
        self.matrix_entries.clear()
        self.matrix2_entries.clear()
        try:
            rows = int(self.m_rows.get())
            cols = int(self.m_cols.get())
            if rows <= 0 or cols <= 0:
                raise ValueError
        except:
            messagebox.showerror("Ошибка", "Введите положительные целые числа")
            return
        tk.Label(self.matrix_frame, text="Матрица A:").grid(row=0, column=0, columnspan=cols)
        self.matrix_entries = [[tk.Entry(self.matrix_frame, width=6) for _ in range(cols)] for __ in range(rows)]
        for i in range(rows):
            for j in range(cols):
                self.matrix_entries[i][j].grid(row=i+1, column=j)
                self.matrix_entries[i][j].insert(0, "0")
        tk.Label(self.matrix_frame, text="Матрица B (для слож/выч/умн):").grid(row=rows+2, column=0, columnspan=cols)
        self.matrix2_entries = [[tk.Entry(self.matrix_frame, width=6) for _ in range(cols)] for __ in range(rows)]
        for i in range(rows):
            for j in range(cols):
                self.matrix2_entries[i][j].grid(row=rows+3+i, column=j)
                self.matrix2_entries[i][j].insert(0, "0")

    def get_matrix(self, entries):
        rows = len(entries)
        cols = len(entries[0]) if rows>0 else 0
        mat = np.zeros((rows, cols))
        for i in range(rows):
            for j in range(cols):
                try:
                    val = float(entries[i][j].get())
                except:
                    val = 0
                mat[i,j] = val
        return mat

    def do_matrix_op(self):
        self.clear_output()
        op = self.m_op.get()
        try:
            rows = int(self.m_rows.get())
            cols = int(self.m_cols.get())
        except:
            messagebox.showerror("Ошибка", "Сначала создайте матрицы")
            return
        A = self.get_matrix(self.matrix_entries)
        if op in ["Сложение", "Вычитание", "Умножение матриц"]:
            B = self.get_matrix(self.matrix2_entries)
        if op == "Сложение":
            if A.shape != B.shape:
                self.log("Ошибка: размеры матриц не совпадают")
                return
            res = A + B
            self.log("Сложение матриц:")
            self.log(f"A + B =\n{res}")
            steps = ""
            for i in range(rows):
                row = "  "
                for j in range(cols):
                    row += f"{A[i,j]} + {B[i,j]} = {res[i,j]}    "
                steps += row + "\n"
            self.log("Промежуточные действия:\n" + steps)
        elif op == "Вычитание":
            if A.shape != B.shape:
                self.log("Ошибка: размеры матриц не совпадают")
                return
            res = A - B
            self.log("Вычитание матриц:")
            self.log(f"A - B =\n{res}")
            steps = ""
            for i in range(rows):
                row = "  "
                for j in range(cols):
                    row += f"{A[i,j]} - {B[i,j]} = {res[i,j]}    "
                steps += row + "\n"
            self.log("Промежуточные действия:\n" + steps)
        elif op == "Умножение на число":
            try:
                scalar = float(self.m_scalar.get())
            except:
                scalar = 1
            res = A * scalar
            self.log(f"Умножение матрицы на {scalar}:")
            self.log(f"{scalar} * A =\n{res}")
            steps = ""
            for i in range(rows):
                row = "  "
                for j in range(cols):
                    row += f"{A[i,j]} * {scalar} = {res[i,j]}    "
                steps += row + "\n"
            self.log("Промежуточные действия:\n" + steps)
        elif op == "Умножение матриц":
            if A.shape[1] != B.shape[0]:
                self.log(f"Ошибка: столбцов A ({A.shape[1]}) не равно строкам B ({B.shape[0]})")
                return
            res = A @ B
            self.log("Умножение матриц A * B:")
            self.log(f"{res}")
            steps = "Результат [i,j] = Σ_k A[i,k]*B[k,j]\n"
            for i in range(res.shape[0]):
                row = "  "
                for j in range(res.shape[1]):
                    row += f"{res[i,j]:.4f}    "
                steps += row + "\n"
            self.log("Промежуточные действия:\n" + steps)
        elif op == "Транспонирование":
            res = A.T
            self.log("Транспонирование матрицы:")
            self.log(f"Исходная A:\n{A}\nТранспонированная A^T:\n{res}")
        elif op == "Определитель":
            if rows != cols:
                self.log("Ошибка: определитель только для квадратной матрицы")
                return
            det = np.linalg.det(A)
            self.log(f"Определитель матрицы A:\n{det:.6f}")
        elif op == "Ранг":
            rank = np.linalg.matrix_rank(A)
            self.log(f"Ранг матрицы A:\n{rank}")
        else:
            self.log("Выберите операцию")

    def create_slau_tab(self):
        tab = ttk.Frame(self.notebook)
        self.notebook.add(tab, text="СЛАУ")
        tk.Label(tab, text="Число уравнений:").grid(row=0, column=0)
        self.slau_n = tk.Entry(tab, width=5)
        self.slau_n.grid(row=0, column=1)
        tk.Button(tab, text="Создать систему", command=self.create_slau_inputs).grid(row=0, column=2)
        self.slau_frame = tk.Frame(tab)
        self.slau_frame.grid(row=1, column=0, columnspan=3, pady=10)
        self.A_entries = []
        self.B_entries = []
        tk.Label(tab, text="Метод:").grid(row=2, column=0)
        self.slau_method = ttk.Combobox(tab, values=["Матричный метод", "Метод Крамера", "Метод Гаусса"])
        self.slau_method.grid(row=2, column=1)
        tk.Button(tab, text="Решить", command=self.solve_slau).grid(row=2, column=2)

    def create_slau_inputs(self):
        for widget in self.slau_frame.winfo_children():
            widget.destroy()
        self.A_entries.clear()
        self.B_entries.clear()
        try:
            n = int(self.slau_n.get())
            if n <= 0:
                raise ValueError
        except:
            messagebox.showerror("Ошибка", "Введите положительное целое число")
            return
        tk.Label(self.slau_frame, text="Матрица A (коэффициенты):").grid(row=0, column=0, columnspan=n)
        self.A_entries = [[tk.Entry(self.slau_frame, width=6) for _ in range(n)] for __ in range(n)]
        for i in range(n):
            for j in range(n):
                self.A_entries[i][j].grid(row=i+1, column=j)
                self.A_entries[i][j].insert(0, "0")
        tk.Label(self.slau_frame, text="Столбец B (свободные члены):").grid(row=n+2, column=0, columnspan=n)
        self.B_entries = [tk.Entry(self.slau_frame, width=6) for _ in range(n)]
        for i in range(n):
            self.B_entries[i].grid(row=n+3+i, column=0, columnspan=n)

    def get_slau_matrices(self):
        n = len(self.A_entries)
        A = np.zeros((n, n))
        B = np.zeros(n)
        for i in range(n):
            for j in range(n):
                try:
                    A[i,j] = float(self.A_entries[i][j].get())
                except:
                    A[i,j] = 0
            try:
                B[i] = float(self.B_entries[i].get())
            except:
                B[i] = 0
        return A, B

    def solve_slau(self):
        self.clear_output()
        method = self.slau_method.get()
        if not method:
            self.log("Выберите метод решения")
            return
        try:
            A, B = self.get_slau_matrices()
            n = A.shape[0]
        except:
            self.log("Сначала создайте систему")
            return
        try:
            if method == "Матричный метод":
                if abs(np.linalg.det(A)) < 1e-10:
                    self.log("Ошибка: матрица вырождена (det=0)")
                    return
                A_inv = np.linalg.inv(A)
                X = A_inv @ B
                self.log(f"Матричный метод: X = A⁻¹·B")
                self.log(f"Обратная матрица:\n{A_inv}")
                self.log(f"Решение:\n{X}")
                self.log("Промежуточные действия: вычислена обратная матрица, затем умножение.")
            elif method == "Метод Крамера":
                detA = np.linalg.det(A)
                if abs(detA) < 1e-10:
                    self.log("Ошибка: определитель = 0, метод Крамера неприменим")
                    return
                X = np.zeros(n)
                steps = f"Δ = {detA:.6f}\n"
                for i in range(n):
                    Ai = A.copy()
                    Ai[:, i] = B
                    detAi = np.linalg.det(Ai)
                    X[i] = detAi / detA
                    steps += f"Δ{i+1} = {detAi:.6f} -> x{i+1} = {X[i]:.6f}\n"
                self.log(f"Метод Крамера:\n{steps}\nРешение: {X}")
            elif method == "Метод Гаусса":
                augmented = np.hstack([A, B.reshape(-1,1)])
                n = A.shape[0]
                steps = "Прямой ход метода Гаусса:\n"
                for i in range(n):
                    pivot = augmented[i,i]
                    if abs(pivot) < 1e-10:
                        for k in range(i+1, n):
                            if abs(augmented[k,i]) > 1e-10:
                                augmented[[i,k]] = augmented[[k,i]]
                                steps += f"Перестановка строк {i+1} и {k+1}\n"
                                pivot = augmented[i,i]
                                break
                        else:
                            self.log("Система вырождена (нет единственного решения)")
                            return
                    augmented[i] = augmented[i] / pivot
                    steps += f"Строка {i+1} делим на {pivot:.4f}: {augmented[i]}\n"
                    for j in range(i+1, n):
                        factor = augmented[j,i]
                        augmented[j] = augmented[j] - factor * augmented[i]
                        steps += f"Строка {j+1} - {factor:.4f} * строка {i+1} -> {augmented[j]}\n"
                X = np.zeros(n)
                steps += "\nОбратный ход:\n"
                for i in range(n-1, -1, -1):
                    X[i] = augmented[i, -1]
                    for j in range(i+1, n):
                        X[i] -= augmented[i, j] * X[j]
                    steps += f"x{i+1} = {X[i]:.6f}\n"
                self.log(f"Метод Гаусса:\n{steps}\nРешение: {X}")
        except Exception as e:
            self.log(f"Ошибка при решении: {str(e)}")

    def create_vector_tab(self):
        tab = ttk.Frame(self.notebook)
        self.notebook.add(tab, text="Векторы")
        tk.Label(tab, text="Размерность векторов:").grid(row=0, column=0)
        self.vec_dim = tk.Entry(tab, width=5)
        self.vec_dim.grid(row=0, column=1)
        self.vec_dim.insert(0, "3")
        tk.Button(tab, text="Создать поля", command=self.create_vector_inputs).grid(row=0, column=2)
        self.vec_frame = tk.Frame(tab)
        self.vec_frame.grid(row=1, column=0, columnspan=4, pady=10)
        self.vec1_entries = []
        self.vec2_entries = []
        self.vec3_entries = []
        tk.Label(tab, text="Операция:").grid(row=2, column=0)
        self.vec_op = ttk.Combobox(tab, values=["Сложение", "Вычитание", "Умножение на число", "Скалярное произведение", "Векторное произведение (3D)", "Смешанное произведение (3D)"])
        self.vec_op.grid(row=2, column=1)
        self.vec_scalar = tk.Entry(tab, width=10)
        self.vec_scalar.grid(row=2, column=2)
        self.vec_scalar.insert(0, "2")
        tk.Button(tab, text="Вычислить", command=self.do_vector_op).grid(row=2, column=3)

    def create_vector_inputs(self):
        for widget in self.vec_frame.winfo_children():
            widget.destroy()
        self.vec1_entries.clear()
        self.vec2_entries.clear()
        self.vec3_entries.clear()
        try:
            dim = int(self.vec_dim.get())
            if dim <= 0:
                raise ValueError
        except:
            messagebox.showerror("Ошибка", "Введите положительное целое число")
            return
        tk.Label(self.vec_frame, text="Вектор a:").grid(row=0, column=0)
        self.vec1_entries = [tk.Entry(self.vec_frame, width=6) for _ in range(dim)]
        for i in range(dim):
            self.vec1_entries[i].grid(row=0, column=i+1)
            self.vec1_entries[i].insert(0, "0")
        tk.Label(self.vec_frame, text="Вектор b:").grid(row=1, column=0)
        self.vec2_entries = [tk.Entry(self.vec_frame, width=6) for _ in range(dim)]
        for i in range(dim):
            self.vec2_entries[i].grid(row=1, column=i+1)
            self.vec2_entries[i].insert(0, "0")
        tk.Label(self.vec_frame, text="Вектор c (для смешанного):").grid(row=2, column=0)
        self.vec3_entries = [tk.Entry(self.vec_frame, width=6) for _ in range(dim)]
        for i in range(dim):
            self.vec3_entries[i].grid(row=2, column=i+1)
            self.vec3_entries[i].insert(0, "0")

    def get_vector(self, entries):
        return np.array([float(e.get()) for e in entries])

    def do_vector_op(self):
        self.clear_output()
        op = self.vec_op.get()
        if not op:
            self.log("Выберите операцию")
            return
        try:
            dim = int(self.vec_dim.get())
            a = self.get_vector(self.vec1_entries)
            b = self.get_vector(self.vec2_entries)
            if dim != len(a) or dim != len(b):
                self.log("Ошибка размерности")
                return
        except:
            self.log("Сначала создайте векторы")
            return
        if op == "Сложение":
            res = a + b
            self.log(f"Сложение векторов a + b = {res}")
            steps = "  ".join(f"{a[i]} + {b[i]} = {res[i]}" for i in range(dim))
            self.log(f"Промежуточные действия: {steps}")
        elif op == "Вычитание":
            res = a - b
            self.log(f"Вычитание векторов a - b = {res}")
            steps = "  ".join(f"{a[i]} - {b[i]} = {res[i]}" for i in range(dim))
            self.log(f"Промежуточные действия: {steps}")
        elif op == "Умножение на число":
            try:
                scalar = float(self.vec_scalar.get())
            except:
                scalar = 1
            res = a * scalar
            self.log(f"Умножение вектора a на {scalar}: {res}")
            steps = "  ".join(f"{a[i]} * {scalar} = {res[i]}" for i in range(dim))
            self.log(f"Промежуточные действия: {steps}")
        elif op == "Скалярное произведение":
            dot = np.dot(a, b)
            self.log(f"Скалярное произведение a·b = {dot}")
            steps = " + ".join(f"{a[i]}*{b[i]}" for i in range(dim))
            self.log(f"Промежуточные действия: {steps} = {dot}")
        elif op == "Векторное произведение (3D)":
            if dim != 3:
                self.log("Векторное произведение определено только для 3D векторов")
                return
            cross = np.cross(a, b)
            self.log(f"Векторное произведение a × b = {cross}")
            self.log("Промежуточные действия: i*(a2b3 - a3b2) - j*(a1b3 - a3b1) + k*(a1b2 - a2b1)")
        elif op == "Смешанное произведение (3D)":
            if dim != 3:
                self.log("Смешанное произведение определено только для 3D векторов")
                return
            try:
                c = self.get_vector(self.vec3_entries)
            except:
                self.log("Ошибка получения вектора c")
                return
            mixed = np.dot(a, np.cross(b, c))
            self.log(f"Смешанное произведение (a, b, c) = {mixed}")
            self.log("Промежуточные действия: a·(b×c)")
        else:
            self.log("Неизвестная операция")

if __name__ == "__main__":
    root = tk.Tk()
    app = MatrixVectorCalculator(root)
    root.mainloop()