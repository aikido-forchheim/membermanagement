using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AVF.MemberManagement.Reports
{
    class UndoRedoStack
    {
        private int              m_UndoRedoIndex;
        private List<ReportBase> m_UndoRedoStack;
        private Form             m_formParent;
        private Control          m_ctrlUndo;
        private Control          m_ctrlRedo;

        public UndoRedoStack(Form formParent, Control ctrlUndo, Control ctrlRedo)
        {
            m_formParent = formParent;
            m_ctrlUndo = ctrlUndo;
            m_ctrlRedo = ctrlRedo;
            m_UndoRedoStack = new List<ReportBase>();
            m_UndoRedoIndex = -1;
        }

        public void Add(ReportBase panelNew)
        {
            if (!IsEmpty())
            {
                SkipTail();
                UnLoad();
            }
            m_UndoRedoStack.Add(panelNew);
            IncUndoRedoIndex();
            Load();
        }

        public void Undo()
        {
            if (!IsAtStart())
            {
                UnLoad();
                DecUndoRedoIndex();
                Load();
            }
        }

        public void Redo()
        {
            if (!IsAtEnd())
            {
                UnLoad();
                IncUndoRedoIndex();
                Load();
            }
        }

        public ReportBase GetActualReport()
            => m_UndoRedoStack[m_UndoRedoIndex];

        private void Load()
            => m_formParent.Controls.Add(GetActualReport());

        private void UnLoad()
            => m_formParent.Controls.Remove(GetActualReport());

        private bool IsEmpty()
            => m_UndoRedoIndex < 0;

        private bool IsAtStart()
            => m_UndoRedoIndex == 0;

        private bool IsAtEnd()
            => m_UndoRedoIndex == m_UndoRedoStack.Count - 1;

        private void SkipTail()
            => m_UndoRedoStack.RemoveRange(m_UndoRedoIndex + 1, m_UndoRedoStack.Count - m_UndoRedoIndex - 1);

        private void SetControls()
        {
            m_ctrlRedo.Enabled = !IsAtEnd();
            m_ctrlUndo.Enabled = !IsAtStart();
        }

        private void IncUndoRedoIndex()
        {
            Debug.Assert(!IsAtEnd());
            m_UndoRedoIndex++;
            SetControls();
        }

        private void DecUndoRedoIndex()
        {
            Debug.Assert(!IsAtStart());
            m_UndoRedoIndex--;
            SetControls();
        }
    }
}
