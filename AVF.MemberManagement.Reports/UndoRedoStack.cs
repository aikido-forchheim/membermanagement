using System.Windows.Forms;
using System.Collections.Generic;

namespace AVF.MemberManagement.Reports
{
    class UndoRedoStack
    {
        public ReportBase P_reportActual { get; private set; }

        private Stack<ReportBase> m_UndoStack;
        private Stack<ReportBase> m_RedoStack;
        private Form              m_formParent;
        private Control           m_ctrlUndo;
        private Control           m_ctrlRedo;

        public UndoRedoStack(Form formParent, Control ctrlUndo, Control ctrlRedo, ReportBase reportActual)
        {
            m_formParent = formParent;
            m_ctrlUndo = ctrlUndo;
            m_ctrlRedo = ctrlRedo;
            m_RedoStack = new Stack<ReportBase>();
            m_UndoStack = new Stack<ReportBase>();
            SwitchTo(reportActual);
        }

        public void Add(ReportBase reportNew)
        {
            m_RedoStack.Clear();  // Once we issue a new report, the redo stack clears 
            m_UndoStack.Push(P_reportActual);
            SwitchTo(reportNew);
        }

        public void Undo()
        {
            if (m_UndoStack.Count > 0)
            {
                m_RedoStack.Push(P_reportActual);
                SwitchTo(m_UndoStack.Pop());
            }
        }

        public void Redo()
        {
            if (m_RedoStack.Count > 0)
            {
                m_UndoStack.Push(P_reportActual);
                SwitchTo(m_RedoStack.Pop());
            }
        }

        private void SwitchTo(ReportBase reportNew)
        {
            m_formParent.Controls.Remove(P_reportActual);
            P_reportActual = reportNew;
            m_formParent.Controls.Add(P_reportActual);
            m_ctrlRedo.Enabled = m_RedoStack.Count > 0;
            m_ctrlUndo.Enabled = m_UndoStack.Count > 0;
        }
    }
}
