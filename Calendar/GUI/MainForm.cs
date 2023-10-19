using BL.DTO;
using BL.ForAPI.Interfaces;
using NLog;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace GUI
{
    public partial class MainForm : Form
    {
        private GUI gui;
        private DateTime from;
        private DateTime to;
        private List<Button> buttons = new List<Button>();
        private List<string> dayString = new List<string>();
        public MainForm(GUI gui)
        {
            for (int i = 0; i < 28; i++)
                dayString.Add("");
            this.gui = gui;
            int shift = (DateTime.Now.DayOfWeek - DayOfWeek.Monday + 7) % 7;
            from = DateTime.Now.Subtract(new TimeSpan(shift, 0, 0, 0));
            from = new DateTime(from.Year, from.Month, from.Day);
            to = from.AddDays(27);
            InitializeComponent();
            buttons.Add(cbutton1);
            buttons.Add(cbutton2);
            buttons.Add(cbutton3);
            buttons.Add(cbutton4);
            buttons.Add(cbutton5);
            buttons.Add(cbutton6);
            buttons.Add(cbutton7);
            buttons.Add(cbutton8);
            buttons.Add(cbutton9);
            buttons.Add(cbutton10);
            buttons.Add(cbutton11);
            buttons.Add(cbutton12);
            buttons.Add(cbutton13);
            buttons.Add(cbutton14);
            buttons.Add(cbutton15);
            buttons.Add(cbutton16);
            buttons.Add(cbutton17);
            buttons.Add(cbutton18);
            buttons.Add(cbutton19);
            buttons.Add(cbutton20);
            buttons.Add(cbutton21);
            buttons.Add(cbutton22);
            buttons.Add(cbutton23);
            buttons.Add(cbutton24);
            buttons.Add(cbutton25);
            buttons.Add(cbutton26);
            buttons.Add(cbutton27);
            buttons.Add(cbutton28);
        }

        private void tAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (tGet.Checked)
                {
                    gui.OnEvGetTask(new EventArgsDisciplineTaskNames(tName.Text, tDName.Text));
                }
                else if (tCreate.Checked)
                {
                    var task = new TaskData
                    {
                        Name = tName.Text,
                        Description = tDescription.Text,
                        DisciplineName = tDName.Text,
                        Date = tDate.Value.Date,
                        Cost = (int)tCost.Value,
                        Finished = tFinished.Checked,
                    };
                    gui.OnEvCreateTask(task);
                }
                else if (tUpdate.Checked)
                {
                    var task = new TaskData
                    {
                        Name = tName.Text,
                        Description = tDescription.Text,
                        DisciplineName = tDName.Text,
                        Date = tDate.Value.Date,
                        Cost = (int)tCost.Value,
                        Finished = tFinished.Checked,
                    };
                    gui.OnEvUpdateTask(task);
                }
                else if (tDelete.Checked)
                {
                    gui.OnEvDeleteTask(new EventArgsDisciplineTaskNames(tName.Text, tDName.Text));
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
            try
            {
                gui.OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
                gui.OnEvGetHolidaysFromTo(new EventArgsGetTasksFromTo(from, to));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }

        private void allDTasks_Click(object sender, EventArgs e)
        {
            try
            {
                gui.OnEvGetDTasks(dName.Text);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }

        private void allDisciplines_Click(object sender, EventArgs e)
        {
            try
            {
                gui.OnEvGetDisciplines(new EventArgs());
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }

        private void allNotifications_Click(object sender, EventArgs e)
        {
            try
            {
                gui.OnEvGetAllCalendars(new EventArgs());
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }



        private void nAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (nGet.Checked)
                {
                    gui.OnEvGetCalendar((int)nDays.Value);
                }
                else if (nCreate.Checked)
                {
                    CalendarData calendar = new CalendarData((int)nDays.Value, nMessage.Text);
                    gui.OnEvCreateCalendar(calendar);
                }
                else if (nUpdate.Checked)
                {
                    CalendarData calendar = new CalendarData((int)nDays.Value, nMessage.Text);
                    gui.OnEvUpdateCalendar(calendar);
                }
                else if (nDelete.Checked)
                {
                    gui.OnEvDeleteCalendar((int)nDays.Value);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }


        private void dAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (dGet.Checked)
                {
                    gui.OnEvGetDiscipline(dName.Text);
                }
                else if (dCreate.Checked)
                {
                    DisciplineData discipline = new DisciplineData
                    {
                        Name = dName.Text,
                        Description = dDescription.Text,
                        Sum = 0
                    };

                    gui.OnEvCreateDiscipline(discipline);
                }
                else if (dUpdate.Checked)
                {
                    DisciplineData discipline = new DisciplineData
                    {
                        Name = dName.Text,
                        Description = dDescription.Text,
                        Sum = 0
                    };
                    gui.OnEvUpdateDiscipline(discipline);
                }
                else if (dDelete.Checked)
                {
                    gui.OnEvDeleteDiscipline(dName.Text);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }

        private void hAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (hGet.Checked)
                {
                    gui.OnEvGetHoliday(hDate.Value.Date);
                }
                else if (hCreate.Checked)
                {
                    gui.OnEvCreateHoliday(new HolidayData(hDate.Value.Date, hMessage.Text));
                }
                else if (hUpdate.Checked)
                {
                    gui.OnEvUpdateHoliday(new HolidayData(hDate.Value.Date, hMessage.Text));
                }
                else if (hDelete.Checked)
                {
                    gui.OnEvDeleteHoliday(hDate.Value.Date);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
            try
            {
                gui.OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
                gui.OnEvGetHolidaysFromTo(new EventArgsGetTasksFromTo(from, to));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex);
                ShowWarning("Что-то пошло не так. Повторите попытку");
            }
        }

        private string? notification;
        public void CalendarNotification(string notification)
        {
            this.notification = notification;
        }
        public void ShowWarning(string warning)
        {
            Console.WriteLine(notification);
            MessageBox.Show(this, warning, "Warning");
        }
        public void ShowCalendar(CalendarData calendar)
        {
            nMessage.Text = calendar.Message;
        }
        public void ShowCalendars(List<CalendarData> calendars)
        {
            string text = "";
            foreach (CalendarData calendar in calendars)
                text += $"Количество дней до оповещания: {calendar.Day}\r\nТекст оповещания: {calendar.Message}\r\n\r\n";
            TextForm f = new();
            f.display.Text = text;
            f.Show();
        }
        public void ShowTasksFromTo(List<TaskData>? tasks)
        {
            tasks?.Sort((TaskData task1, TaskData task2) => task1.Date.CompareTo(task2.Date));
            DateTime date = from;
            int taskIndex = 0;
            foreach (var button in buttons)
            {
                int dayStringIndex = date.Subtract(from).Days;
                dayString[dayStringIndex] = "";
                int finished = 0;
                int notFinished = 0;
                button.Text = $"{date.Day}.{date.Month}.{date.Year}\r\n{date.DayOfWeek}\r\n";
                while (tasks != null && taskIndex < tasks.Count && tasks[taskIndex].Date == date)
                {
                    var task = tasks[taskIndex];
                    dayString[dayStringIndex] += $"название: {task.Name}\r\nназвание дисциплины: {task.DisciplineName}\r\nописание: {task.Description}\r\nколичество баллов: {task.Cost}\r\nзавершено: {finished}\r\n\r\n";
                    if (tasks[taskIndex].Finished)
                        finished++;
                    else
                        notFinished++;
                    taskIndex++;
                }
                if (dayString[dayStringIndex] == "")
                    dayString[dayStringIndex] = "Нет задач на этот день";
                button.Text += $"Выполнено: {finished}\r\nНе выполнено:{notFinished}";
                if (finished > 0 && notFinished == 0)
                    button.BackColor = Color.Green;
                else if (notFinished > 0 && finished == 0)
                    button.BackColor = Color.Red;
                else if (finished + notFinished > 0)
                    button.BackColor = Color.Yellow;
                else
                    button.BackColor = SystemColors.Control;
                date = date.AddDays(1);
            }
        }

        public void ShowDisciplines(List<DisciplineData> disciplines)
        {
            string text = "";
            foreach (DisciplineData discipline in disciplines)
                text += $"название: {discipline.Name}\r\nописание: {discipline.Description}\r\nсумма баллов: {discipline.Sum}\r\n\r\n";
            TextForm f = new();
            f.display.Text = text;
            f.Show();
        }
        public void ShowDiscipline(DisciplineData discipline)
        {
            dName.Text = discipline.Name;
            dDescription.Text = discipline.Description;
            dSum.Text = discipline.Sum.ToString();
        }
        public void ShowDTasks(List<TaskData>? tasks)
        {
            string text = "";
            if (tasks == null)
                text += "Для данной дисциплины ещё не создано задач";
            else
            {
                text += $"Список задач для дисциплины \"{tasks[0].DisciplineName}\":\r\n";
                foreach (var task in tasks)
                {
                    var finished = task.Finished ? "да" : "нет";
                    text += $"\tназвание: {task.Name}\r\n\tописание: {task.Description}\r\n\tсрок: {task.Date.Day}.{task.Date.Month}.{task.Date.Year}\r\n\tколичество баллов: {task.Cost}\r\n\tзавершено: {finished}\r\n\r\n";
                }
            }
            TextForm f = new();
            f.display.Text = text;
            f.Show();
        }
        public void ShowTask(TaskData task)
        {
            tName.Text = task.Name;
            tDescription.Text = task.Description;
            tDName.Text = task.DisciplineName;
            tDate.Value = task.Date;
            tCost.Value = task.Cost;
            tFinished.Checked = task.Finished;
        }

        public void ShowHoliday(HolidayData holiday)
        {
            hDate.Value = holiday.Date;
            hMessage.Text = holiday.Message;
        }
        public void ShowHolidaysFromTo(List<HolidayData>? holidays)
        {
            if (holidays == null)
                return;
            foreach (var holiday in holidays)
            {
                int buttonIndex = holiday.Date.Subtract(from).Days;
                dayString[buttonIndex] = holiday.Message;
                buttons[buttonIndex].Text = $"{holiday.Date.Day}.{holiday.Date.Month}.{holiday.Date.Year}\r\n{holiday.Date.DayOfWeek}\r\n\r\nВыходной\r\n\r\n";
                buttons[buttonIndex].BackColor = Color.Pink;
            }
        }

        private void MouseMove_Event(object sender, MouseEventArgs e)
        {
            if (notification != null)
            {
                TextForm f = new();
                f.display.Text = notification;
                notification = null;
                f.Show();
            }
        }


        private void cbutton1_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[0];
            f.Show();
        }
        private void cbutton2_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[1];
            f.Show();
        }
        private void cbutton3_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[2];
            f.Show();
        }
        private void cbutton4_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[3];
            f.Show();
        }
        private void cbutton5_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[4];
            f.Show();
        }

        private void cbutton6_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[5];
            f.Show();
        }

        private void cbutton7_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[6];
            f.Show();
        }

        private void cbutton8_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[7];
            f.Show();
        }

        private void cbutton9_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[8];
            f.Show();
        }

        private void cbutton10_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[9];
            f.Show();
        }

        private void cbutton11_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[10];
            f.Show();
        }

        private void cbutton12_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[11];
            f.Show();
        }

        private void cbutton13_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[12];
            f.Show();
        }

        private void cbutton14_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[13];
            f.Show();
        }

        private void cbutton15_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[14];
            f.Show();
        }

        private void cbutton16_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[15];
            f.Show();
        }

        private void cbutton17_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[16];
            f.Show();
        }

        private void cbutton18_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[17];
            f.Show();
        }

        private void cbutton19_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[18];
            f.Show();
        }

        private void cbutton20_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[19];
            f.Show();
        }

        private void cbutton21_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[20];
            f.Show();
        }

        private void cbutton22_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[21];
            f.Show();
        }

        private void cbutton23_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[22];
            f.Show();
        }

        private void cbutton24_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[23];
            f.Show();
        }

        private void cbutton25_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[24];
            f.Show();
        }

        private void cbutton26_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[25];
            f.Show();
        }

        private void cbutton27_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[26];
            f.Show();
        }

        private void cbutton28_Click(object sender, EventArgs e)
        {
            TextForm f = new();
            f.display.Text = dayString[27];
            f.Show();
        }

        private void next_Click(object sender, EventArgs e)
        {
            from = from.AddDays(28);
            to = to.AddDays(28);
            gui.OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
            gui.OnEvGetHolidaysFromTo(new EventArgsGetTasksFromTo(from, to));
        }

        private void before_Click(object sender, EventArgs e)
        {
            from = from.Subtract(new TimeSpan(28, 0, 0, 0));
            to = to.Subtract(new TimeSpan(28, 0, 0, 0));
            gui.OnEvGetTasksFromTo(new EventArgsGetTasksFromTo(from, to));
            gui.OnEvGetHolidaysFromTo(new EventArgsGetTasksFromTo(from, to));
        }

        
    }
}