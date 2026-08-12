using System;
using System.Collections;
using Config;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ApiClient : MonoBehaviour
    {
        public IEnumerator Get<T>(string endpoint, Action<T> onSuccess, Action<Exception> onError = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(ApiConfig.BaseUrl + endpoint);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(new Exception(request.error));
                yield break;
            }
            
            
            T result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            
            onSuccess?.Invoke(result);
        }

        public IEnumerator Post<TRequest, TResponse>(string endpoint, TRequest body, Action<TResponse> onSuccess,
            Action<Exception> onError = null)
        {
            string json = JsonConvert.SerializeObject(body);
            
            UnityWebRequest request = UnityWebRequest.Post(ApiConfig.BaseUrl + endpoint, json, "application/json");
            
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(new Exception(request.error));
                yield break;
            }
            
            TResponse response = JsonConvert.DeserializeObject<TResponse>(request.downloadHandler.text);
            
            onSuccess?.Invoke(response);
        }

        public IEnumerator Post<TRequest>(string endpoint, TRequest body, Action onSuccess,
            Action<Exception> onError = null)
        {
            string json = JsonConvert.SerializeObject(body);
            
            UnityWebRequest request = UnityWebRequest.Post(ApiConfig.BaseUrl + endpoint, json, "application/json");
            
            yield return request.SendWebRequest();
            
            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(new Exception(request.error));
                yield break;
            }
            
            onSuccess?.Invoke();
            
        }
        
    }
}