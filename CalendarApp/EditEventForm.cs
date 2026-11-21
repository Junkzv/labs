using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalendarApp
{
    public class EditEventForm : Form
    {
        public DateTime SelectedDate { get; private set; }
        public string NewDescription { get; private set; }

        private DateTimePicker datePicker;
        private TextBox descriptionTextBox;
        private Button okButton;
        private Button cancelButton;

        public EditEventForm(DateTime currentDate, string currentDescription)
        {
            this.Text = "Редактирование события";
            this.Width = 320;
            this.Height = 180;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var dateLabel = new Label { Text = "Дата:", Location = new Point(10, 12), AutoSize = true };
            datePicker = new DateTimePicker
            {
                Location = new Point(10, 30),
                Width = 280,
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today,
                Value = currentDate >= DateTime.Today ? currentDate : DateTime.Today
            };

            var descLabel = new Label { Text = "Описание:", Location = new Point(10, 60), AutoSize = true };
            descriptionTextBox = new TextBox
            {
                Location = new Point(10, 78),
                Width = 280,
                Text = currentDescription
            };

            okButton = new Button
            {
                Text = "Сохранить",
                Location = new Point(10, 112),
                Width = 100,
                DialogResult = DialogResult.OK
            };
            okButton.Click += (s, e) =>
            {
                SelectedDate = datePicker.Value.Date;
                NewDescription = descriptionTextBox.Text.Trim();
                this.Close();
            };

            cancelButton = new Button
            {
                Text = "Отмена",
                Location = new Point(120, 112),
                Width = 80,
                DialogResult = DialogResult.Cancel
            };
            cancelButton.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                dateLabel, datePicker, descLabel, descriptionTextBox, okButton, cancelButton
            });

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }
    }
}
