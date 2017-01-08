using System.Drawing;
using System.Windows.Forms;

namespace OP.GG_Scrap
{
    static class Invoker
    {

        public enum Mode
        {
            SetMaximum = 0,
            SetValue = 1

        }

        //All Controls
        private delegate void _ChangeVisible(Control ctrl, bool state);
        private delegate void _EnableDisable(Control ctrl, bool state);
        private delegate void _ChangeText(Control ctrl, string newText);

        //Forms
        private delegate void _ChangeVisibilityForm(Form frm, bool state);

        //RichTextBox
        private delegate void _AppendText(RichTextBox ctrl, string newText);

        //NumericUpDown
        private delegate void _SetNumericValue(NumericUpDown ctrl, decimal val);

        //Label
        private delegate void _SetLabelText(Label label, string msg, string clr);

        //Progressbar
        private delegate void _SetPrgbValue(ProgressBar prgb, int value, Mode mode);

        //ListView
        private delegate void _AddListItem(ListView list, string value, bool subItem = false, string subValue = "", string tag = "");
        private delegate void _ClearListView(ListView list);
        private delegate void _UpdateListView(ListView list, bool start = true);
        public static void AppendText(RichTextBox ctrl, string newText)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new _AppendText(AppendText), ctrl, newText);
            }
            else
            {
                ctrl.AppendText(System.Environment.NewLine);
                ctrl.AppendText(newText);
                ctrl.ScrollToCaret();
            }
        }
        public static void ChangeText(Control ctrl, string newText)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new _ChangeText(ChangeText), ctrl, newText);
            }
            else
            {
                ctrl.Text = newText;
            }
        }
        public static void EnableDisable(Control ctrl, bool state)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new _EnableDisable(EnableDisable), ctrl, state);
            }
            else
            {
                ctrl.Enabled = state;
            }
        }
        public static void ChangeVisibilityForm(Form frm, bool state)
        {
            if (frm.InvokeRequired)
            {
                frm.Invoke(new _ChangeVisibilityForm(ChangeVisibilityForm), frm, state);
            }
            else
            {
                if (state)
                {
                    frm.Show();
                }
                else
                {
                    frm.Hide();
                }
            }
        }
        public static void SetNumericValue(NumericUpDown ctrl, decimal val)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new _SetNumericValue(SetNumericValue), ctrl, val);
            }
            else
            {
                ctrl.Value = val;
            }
        }
        public static void ChangeVisible(Control ctrl, bool state)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new _ChangeVisible(ChangeVisible), ctrl, state);
            }
            else
            {
                ctrl.Visible = state;
            }
        }
        public static void SetLabelText(Label label, string msg, string clr = null)
        {

            if (label.InvokeRequired)
            {
                label.Invoke(new _SetLabelText(SetLabelText), label, msg, clr);
            }
            else
            {
                label.Text = msg;

                if (clr != null)
                {
                    label.ForeColor = ColorTranslator.FromHtml(clr);
                }
            }

        }
        public static void UpdateList(ListView list, bool start = true)
        {
            if (list.InvokeRequired)
            {
                list.Invoke(new _UpdateListView(UpdateList), list, start);
            }
            else
            {
                if (start)
                {
                    list.BeginUpdate();
                }
                else
                {
                    list.EndUpdate();
                }
            }

        }
        public static void ClearList(ListView list)
        {
            if (list.InvokeRequired)
            {
                list.Invoke(new _ClearListView(ClearList), list);
            }
            else
            {
                list.Items.Clear();
            }

        }
        public static void AddListItem(ListView list, string value, bool subItem = false, string subValue = "", string tag = "")
        {
            if (list.InvokeRequired)
            {
                list.Invoke(new _AddListItem(AddListItem), list, value, subItem, subValue, tag);
            }
            else
            {
                ListViewItem itm = new ListViewItem();
                itm.Tag = tag;
                itm.Text = value;
                if (subItem)
                {
                    itm.SubItems.Add(subValue);
                }
                list.Items.Add(itm);
                //list.AutoResizeColumns(System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
            }

        }

        public static void SetPrgbState(ProgressBar prgb, int value, Mode mode)
        {
            if (prgb.InvokeRequired)
            {
                prgb.Invoke(new _SetPrgbValue(SetPrgbState), prgb, value, mode);
            }
            else
            {
                if (mode == Mode.SetMaximum)
                {
                    prgb.Maximum = value;
                }
                else if (mode == Mode.SetValue)
                {
                    prgb.Value = value;
                }

            }

        }

    }
}
