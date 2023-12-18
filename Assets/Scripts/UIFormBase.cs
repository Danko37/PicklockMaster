using UnityEngine;

//Хранение общей логики для всех форм
public class UIFormBase : MonoBehaviour
{
    protected virtual void OnAwakeForm()
    {
    }
    
    protected virtual void OnShowForm()
    {
    }
    
    protected virtual void OnHideForm()
    {
        
    }
    
    protected virtual void OnDestroyForm()
    {
    }

    /// <summary>
    /// Метод сетапит зависимости для формы
    /// </summary>
    protected virtual void InitServices()
    {
        
    }
}
