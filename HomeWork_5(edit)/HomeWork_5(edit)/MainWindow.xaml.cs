using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HomeWork_5_edit_
{
    class Employee
    {
        public enum Department
        {
            LabourProtection,
            Production,
            Engineering,
            Any
        }

        public enum Position
        {
            Electrician,
            Machinist,
            Engineer,
            Any
        }

        public int employeeID;
        public float employeeSalary;
        public string employeeName;
        public Department department;
        public Position position;

        public int EmployeeID
        {
            set
            {
                if (value != 0)
                    employeeID = value;
            }
            get
            {
                return employeeID;
            }
        }

        public float EmployeeSalary
        {
            set
            {
                if (value > 0)
                    employeeSalary = value;
            }
            get
            {
                return employeeSalary;
            }
        }

        public string EmployeeName
        {
            set
            {
                if (value != null)
                    employeeName = value;
            }
            get
            {
                return employeeName;
            }
        }

        public override string ToString()
        {
            string department_name;
            string position_name;
            string position_intern;

            switch (department)
            {
                case Department.LabourProtection:
                    department_name = "Отдел охраны труда";
                    break;
                case Department.Production:
                    department_name = "Производственный отдел";
                    break;
                case Department.Engineering:
                    department_name = "Конструкторский отдел";
                    break;
                default:
                    department_name = "";
                    break;
            }
            switch (position)
            {
                case Position.Electrician:
                    position_name = "Электрик";
                    break;
                case Position.Engineer:
                    position_name = "Инженер";
                    break;
                case Position.Machinist:
                    position_name = "Машинист-обходчик";
                    break;
                default:
                    position_name = "";
                    break;
            }
            if (employeeSalary < 12000)
                position_intern = "(Стажёр) ";
            else
                position_intern = "";

            return $"{position_intern}{position_name} {employeeName}, {department_name}, Идентификационный номер: {employeeID}, Оклад: {employeeSalary}₽.";
        }
    }
 
    public partial class MainWindow : Window
    {
        static List<Employee> employees = new List<Employee>
        {
            new Employee { employeeID = 8191719, employeeSalary = 87000, employeeName = "Платонов Даниил Юрьевич",
                department = Employee.Department.LabourProtection, position = Employee.Position.Engineer },
            new Employee { employeeID = 8191221, employeeSalary = 7800, employeeName = "Фазлутдинова Альбина Ильмеровна",
                department = Employee.Department.Engineering, position = Employee.Position.Machinist },
            new Employee { employeeID = 8191081, employeeSalary = 67200, employeeName = "Черопко Владимир Алексеевич",
                department = Employee.Department.Production, position = Employee.Position.Machinist },
            new Employee { employeeID = 8191521, employeeSalary = 125400, employeeName = "Тарасов Кирилл Викторович",
                department = Employee.Department.Production, position = Employee.Position.Engineer },
            new Employee { employeeID = 8192271, employeeSalary = 38900, employeeName = "Рыбаков Александр Алексеевич",
                department = Employee.Department.LabourProtection, position = Employee.Position.Electrician },
        };

        public void updateEmployeeList()
        {
            lbEmployees.Items.Clear();
            var selectedDepartment = (Employee.Department)cbDepartmentFilter.SelectedIndex;
            foreach (var employee in employees)
            {
                if (employee.department == selectedDepartment || selectedDepartment == Employee.Department.Any)
                    lbEmployees.Items.Add(employee);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            updateEmployeeList();
        }

        private void cbDepartmentFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateEmployeeList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (float.Parse(tbSalary.Text.Replace('.', ',')) > 0)
                {
                    Employee employee = new Employee
                    {
                        employeeName = tbName.Text,
                        employeeID = int.Parse(tbID.Text),
                        employeeSalary = float.Parse(tbSalary.Text.Replace('.', ',')),
                        department = (Employee.Department)cbDepartment.SelectedIndex,
                        position = (Employee.Position)cbPosition.SelectedIndex
                    };
                    employees.Add(employee);
                    updateEmployeeList();
                }
                else if (float.Parse(tbSalary.Text.Replace('.', ',')) == 0)
                    MessageBox.Show("Будьте гуманны! Заплатите хоть что-нибудь!");
                else
                    MessageBox.Show("Оклад не может быть отрицательным!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте введенные значения", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обратитесь к разработчику: " + ex.Message, "Неизвестная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            employees = employees.OrderBy(s => s.employeeSalary).ToList();
            updateEmployeeList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee[] employees_deleted = new Employee[lbEmployees.SelectedItems.Count];
            lbEmployees.SelectedItems.CopyTo(employees_deleted, 0);

            foreach (var em in employees_deleted)
            {
                lbEmployees.Items.Remove(em);
                employees.Remove(em);
            }
        }

        private void searchEmployee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int idOfEmployee = int.Parse(tbSearchEmployee.Text);
                Employee[] employees_search = new Employee[employees.Count];
                employees.CopyTo(employees_search, 0);

                var idOfSearchEmployee = employees_search.Where(x => x.employeeID == idOfEmployee).ToList();
                int countOfEmployee = idOfSearchEmployee.Count;
                if (countOfEmployee != 0)
                {
                    foreach (var em in employees)
                    {
                        lbEmployees.Items.Remove(em);
                    }

                    foreach (var id in idOfSearchEmployee)
                    {
                        lbEmployees.Items.Add(id);
                    }
                }
                else
                    MessageBox.Show("По данному ID ни одного сотрудника не найдено." + "\nПопробуйте снова.");
            }
            catch (FormatException)
            {
                MessageBox.Show("Проверьте введенные значения", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Обратитесь к разработчику: " + ex.Message, "Неизвестная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            lbEmployees.Items.Clear();
            foreach (var employee in employees)
                lbEmployees.Items.Add(employee);
        }
    }
}
