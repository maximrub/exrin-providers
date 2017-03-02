using Exrin.Abstraction;

namespace Exrin.Base
{
    public class BaseModel : Framework.Model
    {
        public BaseModel(IExrinContainer i_ExrinContainer, IModelState i_ModelState)
            : base(i_ExrinContainer, i_ModelState)
        {
        }
    }
}