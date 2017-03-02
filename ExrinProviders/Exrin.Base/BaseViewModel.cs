using Exrin.Abstraction;

namespace Exrin.Base
{
    public class BaseViewModel : Framework.ViewModel
    {
        public BaseViewModel(
            IExrinContainer i_ExrinContainer,
            IVisualState i_VisualState,
            string i_Caller = nameof(BaseViewModel))
            : base(i_ExrinContainer, i_VisualState, i_Caller)
        {
        }
    }
}
