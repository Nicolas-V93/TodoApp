using System;
using System.Linq;
using System.Windows;
using Pra.Todo.Core;
using Microsoft.Win32;


namespace Pra.Todo.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TodoApp todoApp;
        TodoTask selectedTask;
        bool isNew;

        public MainWindow()
        {
            InitializeComponent();
        }


        #region EventListeners
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            todoApp = new TodoApp();
            DisableControls();
            RetrieveOpenTasks();
        }
        private void LstTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstTasks.SelectedItem == null)
            {
                DisableControls();
                return;
            }
            selectedTask = (TodoTask)lstTasks.SelectedItem;
            ReadTaskData(selectedTask);
            ToggleTaskControls(selectedTask);
        }

        private void BtnComplete_Click(object sender, RoutedEventArgs e)
        {
            ToggleTaskStatus();
        }
        private void BtnReopen_Click(object sender, RoutedEventArgs e)
        {
            ToggleTaskStatus();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            ResetFields();
            ViewAddOrEdit();
            dtpDeadline.SelectedDate = DateTime.Today;
            btnAdd.IsEnabled = false;
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            isNew = false;
            ViewAddOrEdit();
            btnAdd.IsEnabled = false;
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (!todoApp.RemoveTask(selectedTask))
            {
                ShowErrorMessage("Could not delete task");
                return;
            }
            ResetFields();
            ShowTasks();
            lstTasks.SelectedIndex = 0;
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string description = txtDescription.Text;
                DateTime? deadline = dtpDeadline.SelectedDate;

                if(isNew)
                {                
                    TodoTask task = new TodoTask(description, false, deadline);
                    if (!todoApp.Addtask(task))
                    {
                        ShowErrorMessage("Could not add task");
                        return;
                    }
                    ResetFields();
                }
                else
                {
                    selectedTask.Description = description;
                    selectedTask.Deadline = deadline;

                    if (!todoApp.UpdateTask(selectedTask)) 
                    {
                        ShowErrorMessage("Could not update task");
                    }
                }
                ShowTasks();
                DisableControls();
                ViewDefault();
                lstTasks.SelectedItem = selectedTask;
                LstTasks_SelectionChanged(null, null);

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
            
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewDefault();
            LstTasks_SelectionChanged(null, null);
        }

        private void ChkShowCompleted_Click(object sender, RoutedEventArgs e)
        {
            ShowTasks();
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV bestanden (*.csv)|*.csv",
                FileName = "tasks.csv"
            };
            if(saveFileDialog.ShowDialog() == true) // if file is select 'saveFileDialog returns true
            {
                try
                {
                    todoApp.Export(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }

        }
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "CSV bestanden (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    todoApp.Import(openFileDialog.FileName);
                    ShowTasks();
                }
                catch (Exception ex)
                {
                    ShowErrorMessage(ex.Message);
                }
            }
        }
        #endregion


        #region Private Methods

        private void DisableControls()
        {
            btnEdit.IsEnabled = false;
            btnRemove.IsEnabled = false;
            btnComplete.IsEnabled = false;
            btnReopen.IsEnabled = false;
        }
        private void ResetFields()
        {
            txtDescription.Text = "";
            dtpDeadline.SelectedDate = null;
        }
        private void ViewDefault()
        {
            btnAdd.IsEnabled = true;
            lstTasks.IsEnabled = true;
            grpEdit.IsEnabled = false;
        }
        private void ViewAddOrEdit()
        {
            DisableControls();
            grpEdit.IsEnabled = true;
            lstTasks.IsEnabled = false;
            txtDescription.Focus();
            txtDescription.SelectionStart = txtDescription.Text.Length;
        }

        private void RetrieveOpenTasks()
        {
            lstTasks.Items.Clear();
            foreach (TodoTask task in todoApp.OpenTasks)
            {
                lstTasks.Items.Add(task);
            }

        }     
        private void RetrieveAllTasks()
        {
            lstTasks.Items.Clear();
            foreach (TodoTask task in todoApp.Tasks)
            {
                lstTasks.Items.Add(task);
            }

        }
        private void ShowTasks()
        {
            if (chkShowCompleted.IsChecked == true)
            {
                RetrieveAllTasks();
            }
            else
            {
                RetrieveOpenTasks();
            }
        }

        private void ReadTaskData(TodoTask task)
        {
            txtDescription.Text = task.Description;
            dtpDeadline.SelectedDate = task.Deadline;
        }
        private void ToggleTaskControls(TodoTask task)
        {
            btnEdit.IsEnabled = true;
            btnRemove.IsEnabled = true;
            btnComplete.IsEnabled = !task.Completed;
            btnReopen.IsEnabled = task.Completed;
        }
        private void ToggleTaskStatus()
        {
            selectedTask.Completed = !selectedTask.Completed; // beter/altnernative option is to define this Toggle logic inside class library

            if (!todoApp.UpdateTask(selectedTask))
            {
                ShowErrorMessage("Could not update task status");
                return;
            }
            ShowTasks();
            lstTasks.SelectedItem = selectedTask;
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion


    }
}
