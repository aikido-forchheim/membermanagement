using System.Windows.Forms;
using System.Collections.Generic;

namespace AVF.MemberManagement.Reports
{
    public abstract class UndoRedoStack
    {
        public ReportBase P_reportActual { get; private set; }

        private Stack<ReportBase> m_UndoStack;
        private Stack<ReportBase> m_RedoStack;
        private Form              m_formParent;

        protected bool AnyUndoCommands()
            => m_UndoStack.Count > 0;

        protected bool AnyRedoCommands()
            => m_RedoStack.Count > 0;

        public UndoRedoStack(Form formParent)
        {
            m_formParent = formParent;
            m_RedoStack = new Stack<ReportBase>();
            m_UndoStack = new Stack<ReportBase>();
        }

        public void Add(ReportBase reportNew)
        {
            m_RedoStack.Clear();  // Once we issue a new report, the redo stack clears 
            m_UndoStack.Push(P_reportActual);
            SwitchTo(reportNew);
        }

        public void Undo()
        {
            if (AnyUndoCommands())
            {
                m_RedoStack.Push(P_reportActual);
                SwitchTo(m_UndoStack.Pop());
            }
        }

        public void Redo()
        {
            if (AnyRedoCommands())
            {
                m_UndoStack.Push(P_reportActual);
                SwitchTo(m_RedoStack.Pop());
            }
        }

        public abstract void RefreshControls();

        public void SwitchTo(ReportBase reportNew)
        {
            if (P_reportActual != null)
                m_formParent.Controls.Remove(P_reportActual.P_panel);
            P_reportActual = reportNew;
            m_formParent.Controls.Add(P_reportActual.P_panel);
            RefreshControls();
        }
    }
}
