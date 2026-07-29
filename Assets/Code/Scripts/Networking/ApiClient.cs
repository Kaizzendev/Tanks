using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class ApiClient : MonoBehaviour
    {
        private const string BaseUrl = "http://192.168.1.152:5024/";

        public IEnumerator Get<T>(string endpoint, Action<T> onSuccess, Action<Exception> onError = null)
        {
            UnityWebRequest request = UnityWebRequest.Get(BaseUrl + endpoint);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(new Exception(request.error));
                yield break;
            }
            
            T result = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            
            onSuccess?.Invoke(result);
        }
    }
}