using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class IconCacheManager : MonoBehaviour
{
    private readonly Dictionary<string, Sprite> iconCache = new Dictionary<string, Sprite>();
    private AsyncOperationHandle<IList<Sprite>> preloadHandle;

    private const string Item_Icon = "ItemIcon";

    public async Task PreloadAllIconsAsync()
    {
        AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(Item_Icon, typeof(Sprite));
        IList<IResourceLocation> locations = await locationsHandle.Task;

        preloadHandle = Addressables.LoadAssetsAsync<Sprite>(locations, null);
        IList<Sprite> loadedSprites = await preloadHandle.Task;

        for (int i = 0; i < locations.Count; i++)
        {
            string address = locations[i].PrimaryKey;
            if (!iconCache.ContainsKey(address))
            {
                iconCache.Add(address, loadedSprites[i]);
            }
        }

        Addressables.Release(locationsHandle);
    }

    public Sprite GetIcon(string address)
    {
        iconCache.TryGetValue(address, out Sprite icon);
        return icon;
    }

    private void OnDestroy()
    {
        if (preloadHandle.IsValid())
        {
            Addressables.Release(preloadHandle);
        }
        iconCache.Clear();
    }
}
