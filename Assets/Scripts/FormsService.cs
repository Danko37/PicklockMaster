using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FormsService : IGamesService 
{
    private const string UIFormsPrefabPath = "Forms/";

    public Transform FormsParent;

    private List<UIFormBase> _currentShowedForms = new ();

    public FormsService(Transform formsRoot)
    {
        FormsParent = formsRoot;
    }

    public async UniTask Run() { }

    public T ShowHideForm<T>(bool isShow) where T : UIFormBase
    {
        if (isShow)
        {
            if (FormIsShowed<T>())
            {
                Debug.LogWarning($"Form {typeof(T).Name} already showed!!!");
                return GetUIForm<T>();
            }

            var formPath = UIFormsPrefabPath + typeof(T).Name;
            
            //todo сделать прогреф форм
            var form = Resources.Load<T>(formPath);

            if (form == null)
            {
                Debug.LogError($"Form {nameof(T)} is not loaded");
            }

            var spawnedForm = GameObject.Instantiate(form.gameObject, parent: FormsParent).GetComponent<T>();

            _currentShowedForms.Add(spawnedForm);
            
            return spawnedForm ;
        }

        CloseUIForm<T>();
        return null;
    }

    private void CloseUIForm<T>() where T : UIFormBase
    {
        var targetForm = GetUIForm<T>();
        
        if (targetForm == null) return;
        
        _currentShowedForms.Remove(targetForm); 
        GameObject.Destroy(targetForm.gameObject);
    }

    /// <summary>
    /// Возвращает форму, если она показывается
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T GetUIForm<T>() where T : UIFormBase
    {
        if (!FormIsShowed<T>()) return null;
        
        var targetForm = _currentShowedForms.Find(f => f.GetType() == typeof(T));
        return targetForm as T;
    }

    /// <summary>
    /// Форма в данный момент показывается
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private bool FormIsShowed<T>() where T : UIFormBase
    {
        return _currentShowedForms.Exists(f => f.GetType() == typeof(T));
    }

    public void ShowTextPopUp(string title, string body)//может добавить тип собщения: сообщение, предупреждение, критическое
    {
        //создать сам префаб и код мессадж бокса.
        //показать как обычно
        //и заинитить
    }

    public void ShowHidePreloader(bool isShow)
    {
        ShowHideForm<UIFormLoader>(isShow);
    }

    public string SystemName => nameof(FormsService);

    public async UniTask UpdateSystem()
    {
    }
}
