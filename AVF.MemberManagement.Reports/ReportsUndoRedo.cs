
namespace AVF.MemberManagement.Reports
{

    public class ReportsUndoRedo : UndoRedoStack
    {
        private ReportMain m_reportParent;

        public ReportsUndoRedo(ReportMain formParent)
            : base(formParent)
        {
            m_reportParent = formParent;
        }

        public override void RefreshControls()
        {
            m_reportParent.RedoEnabled(AnyRedoCommands());
            m_reportParent.UndoEnabled(AnyUndoCommands());
        }
    }
}
