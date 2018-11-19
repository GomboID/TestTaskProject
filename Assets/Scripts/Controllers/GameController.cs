using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public event System.Action DiactivateAllChainObjects;
    public static GameController Instance = null;

    public GameObject PlayerPrefab;
    public List<GameObject> GameplayObjects;
    public List<Sprite> ActivationChainIcons;
    public List<Image> ActivationChainUI;

    List<GameObject> m_ActivationChain = new List<GameObject>();
    int m_CurrentActivationObjectIndex = 0;
    List<int> m_ActivationChainIndexes;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        Cursor.visible = false;
        SpawnObjects(PlayerPrefab);
        CreateActivationChain();
        foreach (GameObject obj in GameplayObjects)
        {
            SpawnObjects(obj);
        }
        SetUiIcons();
    }


    private void SpawnObjects(GameObject _object)
    {
        float xPos = Random.Range(-25f, 26f);
        float zPos = Random.Range(-25f, 26f);
        if (_object != PlayerPrefab)
            m_ActivationChain.Add(Instantiate(_object, new Vector3(xPos, 0, zPos), new Quaternion(0f, 0f, 0f, 0f)));
        else
            Instantiate(_object, new Vector3(xPos, 0, zPos), new Quaternion(0f, 0f, 0f, 0f));
    }

    private void CreateActivationChain()
    {
        m_ActivationChainIndexes = new List<int>();
        for (int i = 0; i < GameplayObjects.Count; i++)
        {
            m_ActivationChainIndexes.Add(i);
        }
        m_ActivationChainIndexes = m_ActivationChainIndexes.OrderBy(i => Random.value).ToList();
    }

    private void SetUiIcons()
    {
        int index = 0;
        foreach (var UI in ActivationChainUI)
        {
            UI.sprite = ActivationChainIcons[m_ActivationChainIndexes[index]];
            index++;
        }
    }

    public bool ActivateObject(GameObject _object)
    {
        GameObject currentObject = m_ActivationChain[m_ActivationChainIndexes[m_CurrentActivationObjectIndex]];
        if (_object == currentObject)
        {
            m_CurrentActivationObjectIndex++;
            if (m_CurrentActivationObjectIndex == m_ActivationChain.Count)
            {
                ActivationChainSuccess();
            }
            return true;
        }
        else
        {
            if (DiactivateAllChainObjects != null)
                DiactivateAllChainObjects.Invoke();
            m_CurrentActivationObjectIndex = 0;
            return false;
        }
    }

    public void ActivationChainSuccess()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
