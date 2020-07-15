using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagement : MonoBehaviour
{
	public static SceneManagement sceneManagement = null;

	/// <summary>
	/// The currently active scene's build index.
	/// </summary>
	[HideInInspector] public int sceneCurrActive { get; private set; } = -1;

	/// <summary>
	/// List of the build indexes of the previous active scenes.
	/// </summary>
	[HideInInspector] public List<int> scenePrevActive { get; private set; } = new List<int>();

	[Tooltip("The max history size for the list of previous active scenes. Defaults to 30. ")]
	[SerializeField] private int scenePrevActiveLength = 30;

	private void Awake()
	{
		#region Singleton Managment
		if (!sceneManagement)
		{
			sceneManagement = this;
		}

		if (sceneManagement != this)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this);
		#endregion
	}

	private void OnEnable()
	{
		SceneManager.activeSceneChanged += UpdateSceneCurr;
	}

	private void OnDisable()
	{
		SceneManager.activeSceneChanged -= UpdateSceneCurr;
	}

	private void UpdateSceneCurr(Scene scene1, Scene scene2)
	{
		if (sceneCurrActive != -1)
		{
			scenePrevActive.Insert(0, sceneCurrActive);
		}

		if (scenePrevActive.Count > scenePrevActiveLength)
		{
			scenePrevActive.RemoveAt(scenePrevActiveLength);
		}

		sceneCurrActive = scene2.buildIndex;
	}

	public Scene GetCurrScene() { return SceneManager.GetActiveScene(); }

	/// <summary>
	/// Returns array of all loaded scenes.
	/// </summary>
	/// <returns></returns>
	public Scene[] GetAllLoadedScenes()
	{
		Scene[] tempArr = new Scene[SceneManager.sceneCount];

		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			tempArr[i] = SceneManager.GetSceneAt(i);
		}

		return tempArr;
	}

	/// <summary>
	/// Uses the SceneManagement library to load a scene from the build settings. Using Scene Index.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the asynchronously loaded scene has reached 90% completion.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	public void LoadScene(int _sceneIndex, LoadSceneMode _loadAdditive = LoadSceneMode.Single, bool _loadAsync = false, System.Action<AsyncOperation> _onComplete = null, bool _waitToActivate = false)
	{
		if (_loadAsync)
		{
			StartCoroutine(LoadAsync(_sceneIndex, _loadAdditive, _onComplete, _waitToActivate));
		}
		else
		{
			SceneManager.LoadScene(_sceneIndex, _loadAdditive);
		}
	}

	/// <summary>
	/// Uses the SceneManagement library to load a scene from the build settings. Using Scene Name.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the asynchronously loaded scene has reached 90% completion.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	public void LoadScene(string _sceneName, bool _loadAsync = false, LoadSceneMode _loadAdditive = LoadSceneMode.Single,  System.Action<AsyncOperation> _onComplete = null, bool _waitToActivate = false)
	{
		if (_loadAsync)
		{
			StartCoroutine(LoadAsync(_sceneName, _loadAdditive, _onComplete, _waitToActivate));
		}
		else
		{
			SceneManager.LoadScene(_sceneName, _loadAdditive);
		}
	}

	/// <summary>
	/// Uses the SceneManagement library to unload a scene asynchronously. Using Scene Index. You can only unload a scene if isn't the "active" scene. Scenes loaded additively can be unloaded.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the asynchronously loaded scene has reached 90% completion.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	public void UnloadScene(int _sceneIndex, System.Action<AsyncOperation> _onComplete = null, bool _waitToActivate = false)
	{
		StartCoroutine(UnloadAsync(_sceneIndex, _onComplete, _waitToActivate));
	}

	/// <summary>
	/// Uses the SceneManagement library to unload a scene asynchronously. Using Scene Name. You can only unload a scene if isn't the "active" scene. Scenes loaded additively can be unloaded.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the asynchronously loaded scene has reached 90% completion.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	public void UnloadScene(string _sceneName, System.Action<AsyncOperation> _onComplete = null, bool _waitToActivate = false)
	{
		StartCoroutine(UnloadAsync(_sceneName, _onComplete, _waitToActivate));
	}

	/// <summary>
	/// Allows for control over loading a scene asyncronously. This is good for loading feedback. Using Scene Index.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the async process is complete.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	/// <returns></returns>
	private IEnumerator LoadAsync(int _sceneIndex, LoadSceneMode _loadAdditive, System.Action<AsyncOperation> _onComplete, bool _waitToActivate)
	{
		AsyncOperation async;
		async = SceneManager.LoadSceneAsync(_sceneIndex, _loadAdditive);
		if (async == null) // If function fails to call, cancel operation.
			yield break;

		float asyncProgress; // Not being used yet. For sending into functions later if we need.

		if (_waitToActivate)
		{
			async.allowSceneActivation = false;
		}

		while (async.progress < 0.9f)
		{
			asyncProgress = async.progress;
			print("asyncProgress = " + asyncProgress);
			yield return null;
		}

		if (async.progress == 0.9f)
		{
			asyncProgress = 1;
			print("asyncProgress = " + asyncProgress);

			if (_onComplete != null)
			{
				_onComplete(async);
			}
		}

		while (async.allowSceneActivation == false) // Don't allow the async local variable to be lost, or null references will happen if you try to use the _waitToActivate functionality
		{
			yield return null;
		}
	}

	/// <summary>
	/// Allows for control over loading a scene asyncronously. This is good for loading feedback. Using Scene Name.
	/// </summary>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	/// <param name="_onComplete">A function that will be called when the async process is complete.</param>
	/// <returns></returns>
	private IEnumerator LoadAsync(string _sceneName, LoadSceneMode _loadAdditive, System.Action<AsyncOperation> _onComplete, bool _waitToActivate)
	{
		AsyncOperation async;
		async = SceneManager.LoadSceneAsync(_sceneName, _loadAdditive);
		if (async == null) // If function fails to call, cancel operation.
			yield break;

		float asyncProgress; // Not being used yet. For sending into functions later if we need.

		if (_waitToActivate)
		{
			async.allowSceneActivation = false;
		}

		while (async.progress < 0.9f)
		{
			asyncProgress = async.progress;
			print("asyncProgress = " + asyncProgress);
			yield return null;
		}

		if (async.progress == 0.9f)
		{
			asyncProgress = 1;
			print("asyncProgress = " + asyncProgress);

			if (_onComplete != null)
			{
				_onComplete(async);
			}
		}

		while (async.allowSceneActivation == false) // Don't allow the async local variable to be lost, or null references will happen if you try to use the _waitToActivate functionality
		{
			yield return null;
		}
	}

	/// <summary>
	/// Allows for control over unloading a scene asyncronously. This is good for loading feedback. Using Scene Index.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the async process is complete.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	/// <returns></returns>
	private IEnumerator UnloadAsync(int _sceneIndex, System.Action<AsyncOperation> _onComplete, bool _waitToActivate)
	{
		AsyncOperation async;
		async = SceneManager.UnloadSceneAsync(_sceneIndex);
		if (async == null) // If function fails to call, cancel operation.
			yield break;

		float asyncProgress; // Not being used yet. For sending into functions later if we need.

		if (_waitToActivate)
		{
			async.allowSceneActivation = false;
		}

		while (async.progress < 0.9f)
		{
			asyncProgress = async.progress;
			print("asyncProgress = " + asyncProgress);
			yield return null;
		}

		if (async.progress == 0.9f)
		{
			asyncProgress = 1;
			print("asyncProgress = " + asyncProgress);

			if (_onComplete != null)
			{
				_onComplete(async);
			}
		}

		while (async.allowSceneActivation == false) // Don't allow the async local variable to be lost, or null references will happen if you try to use the _waitToActivate functionality
		{
			yield return null;
		}
	}

	/// <summary>
	/// Allows for control over unloading a scene asyncronously. This is good for loading feedback. Using Scene Name.
	/// </summary>
	/// <param name="_onComplete">A function that will be called when the async process is complete.</param>
	/// <param name="_waitToActivate">If this is true, you need to set "allowSceneActivation" to true using the AsyncOperation reference in an _onComplete function you passed through. Otherwise, the scene will never finish loading.</param>
	/// <returns></returns>
	private IEnumerator UnloadAsync(string _sceneName, System.Action<AsyncOperation> _onComplete, bool _waitToActivate)
	{
		AsyncOperation async;
		async = SceneManager.UnloadSceneAsync(_sceneName);
		if (async == null) // If function fails to call, cancel operation.
			yield break;

		float asyncProgress; // Not being used yet. For sending into functions later if we need.

		if (_waitToActivate)
		{
			async.allowSceneActivation = false;
		}

		while (async.progress < 0.9f)
		{
			asyncProgress = async.progress;
			print("asyncProgress = " + asyncProgress);
			yield return null;
		}

		if (async.progress == 0.9f)
		{
			asyncProgress = 1;
			print("asyncProgress = " + asyncProgress);

			if (_onComplete != null)
			{
				_onComplete(async);
			}
		}

		while (async.allowSceneActivation == false) // Don't allow the async local variable to be lost, or null references will happen if you try to use the _waitToActivate functionality
		{
			yield return null;
		}
	}
}