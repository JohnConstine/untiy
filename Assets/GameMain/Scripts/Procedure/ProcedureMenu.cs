using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace SG1
{
    public class ProcedureMenu : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

//            GameEntry.UI.OpenUIForm(UIFormId.TestPage, this);
            GameEntry.UI.OpenUIForm(UIFormId.MainPage, this);
        }
    }
}