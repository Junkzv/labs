using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarApp
{
    public class CalendarForm : Form
    {
        private CalendarManager calendarManager;

        private DateTimePicker datePicker;
        private TextBox descriptionTextBox;
        private Button addEventButton;
        private Button editEventButton;
        private Button removeEventButton;
        private ListBox eventsListBox;
        private Label dateLabel;
        private Label descLabel;
        private Label listLabel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        public CalendarForm()
        {
            InitializeComponents();
            calendarManager = new CalendarManager();
            RefreshList();
        }

        private void InitializeComponents()
        {
            this.Text = "Календарь событий";
            this.Width = 500;
            this.Height = 480;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Font = new Font("Segoe UI", 9f);
            this.BackColor = Color.FromArgb(240, 244, 248);

            // Верхняя панель ввода
            var inputPanel = new Panel
            {
                Location = new Point(10, 10),
                Size = new Size(464, 110),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            dateLabel = new Label
            {
                Text = "Дата события:",
                Location = new Point(10, 10),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            datePicker = new DateTimePicker
            {
                Location = new Point(10, 28),
                Width = 200,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today
            };

            descLabel = new Label
            {
                Text = "Описание:",
                Location = new Point(10, 58),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            descriptionTextBox = new TextBox
            {
                Location = new Point(10, 76),
                Width = 300,
                PlaceholderText = "Введите описание события..."
            };

            addEventButton = new Button
            {
                Location = new Point(320, 73),
                Text = "Добавить",
                Width = 130,
                Height = 26,
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            addEventButton.FlatAppearance.BorderSize = 0;
            addEventButton.Click += AddEventButton_Click;

            inputPanel.Controls.AddRange(new Control[] {
                dateLabel, datePicker, descLabel, descriptionTextBox, addEventButton
            });

            // Список событий
            listLabel = new Label
            {
                Text = "Список событий:",
                Location = new Point(10, 130),
                AutoSize = true,
                ForeColor = Color.FromArgb(60, 60, 60)
            };

            eventsListBox = new ListBox
            {
                Location = new Point(10, 150),
                Width = 464,
                Height = 220,
                Font = new Font("Consolas", 9f),
                BorderStyle = BorderStyle.FixedSingle
            };
            eventsListBox.DoubleClick += (s, e) => EditSelectedEvent();

            // Кнопки управления
            editEventButton = new Button
            {
                Location = new Point(10, 382),
                Text = "✏ Редактировать",
                Width = 150,
                Height = 30,
                BackColor = Color.FromArgb(0, 153, 76),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            editEventButton.FlatAppearance.BorderSize = 0;
            editEventButton.Click += (s, e) => EditSelectedEvent();

            removeEventButton = new Button
            {
                Location = new Point(170, 382),
                Text = "✖ Удалить",
                Width = 120,
                Height = 30,
                BackColor = Color.FromArgb(196, 43, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            removeEventButton.FlatAppearance.BorderSize = 0;
            removeEventButton.Click += RemoveEventButton_Click;

            // Строка статуса
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Готово");
            statusStrip.Items.Add(statusLabel);

            this.Controls.AddRange(new Control[] {
                inputPanel, listLabel, eventsListBox,
                editEventButton, removeEventButton, statusStrip
            });
        }

        private void RefreshList()
        {
            eventsListBox.Items.Clear();
            foreach (var e in calendarManager.Events)
                eventsListBox.Items.Add(e.ToString());

            statusLabel.Text = $"Всего событий: {calendarManager.Events.Count}";
        }

        private void AddEventButton_Click(object sender, EventArgs e)
        {
            try
            {
                var newEvent = new Event(datePicker.Value.Date, descriptionTextBox.Text.Trim());
                calendarManager.AddEvent(newEvent);
                descriptionTextBox.Clear();
                RefreshList();
                statusLabel.Text = "Событие добавлено.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RemoveEventButton_Click(object sender, EventArgs e)
        {
            if (eventsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите событие для удаления!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idx = eventsListBox.SelectedIndex;
            var eventToRemove = calendarManager.Events[idx];

            var result = MessageBox.Show(
                $"Удалить событие:\n{eventToRemove}?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                calendarManager.RemoveEvent(eventToRemove);
                RefreshList();
                statusLabel.Text = "Событие удалено.";
            }
        }

        private void EditSelectedEvent()
        {
            if (eventsListBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите событие для редактирования!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idx = eventsListBox.SelectedIndex;
            var eventToEdit = calendarManager.Events[idx];

            using var editForm = new EditEventForm(eventToEdit.Date, eventToEdit.Description);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    calendarManager.UpdateEvent(eventToEdit, editForm.SelectedDate, editForm.NewDescription);
                    RefreshList();
                    statusLabel.Text = "Событие обновлено.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
