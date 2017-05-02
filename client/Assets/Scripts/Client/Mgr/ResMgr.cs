using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 用法：
 CreateObject的objID用于绑定游戏中的每个单位，清理时候需要传入。
 释放的参数forceRelease为false可以保证游戏对象重用，避免频繁的加载卸载资源内存和CPU性能开销。
 切换场景用ReleaseAll保证没有内存泄露。
 协调好各个管理器，保证资源被卸载后，其它管理器都要在前面卸载。
*/
public class ResMgr : Singleton<ResMgr>
{
    public class AssetItem
    {
        public Object obj;
        public Sprite spr;
        public int refCount;
        public int objID;
        public AssetItem()
        {
            spr = null;
        }
    }
    public Dictionary<string, List<AssetItem>> mResDict = new Dictionary<string, List<AssetItem>>();
    public GameObject CreateObject(string strPath, out int objID)
    {
        GameObject go = null;
        List<AssetItem> itemList = null;
        AssetItem item = null;
        if (mResDict.TryGetValue(strPath, out itemList))
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                // reuse
                if (itemList[i].refCount == 0)
                {
                    item = itemList[i];
                    item.refCount = 1;
                    break;
                }
            }
            // reuse
            if (item == null && itemList.Count > 0)
            {
                item = new AssetItem();
                item.obj = Object.Instantiate(itemList[0].obj);
                item.refCount = 1;
                item.objID = item.obj.GetInstanceID();
                itemList.Add(item);
            }
        }

        if (item == null)
        {
            itemList = new List<AssetItem>();
            item = new AssetItem();
            Object obj = Resources.Load(strPath);
            item.obj = Object.Instantiate(obj);
            item.refCount = 1;
            item.objID = item.obj.GetInstanceID();
            itemList.Add(item);
            mResDict[strPath] = itemList;
        }
        objID = item.objID;
        go = item.obj as GameObject;
        if (go == null)
        {
            Debug.LogError("ResMgr CreateObject faill strPath:" + strPath);
        }
        else
        {
//            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }
        return go;
    }

    public Sprite CreateSprite(string strPath)
    {
        List<AssetItem> itemList = null;
        AssetItem item = null;
        // reuse
        if (mResDict.TryGetValue(strPath, out itemList))
        {
            if (itemList.Count > 0)
            {
                item = itemList[0];
                item.refCount++;
            }
        }
        if (item == null)
        {
            itemList = new List<AssetItem>();
            item = new AssetItem();
            Object obj = Resources.Load(strPath);
            item.obj = obj;
            item.refCount = 1;
            Sprite spr = (item.obj) as Sprite;
            Texture2D tex = item.obj as Texture2D;
            if (spr == null && tex != null)
            {

                spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            }
            if (spr == null)
            {
                Debug.LogError("ResMgr CreateSprite faill strPath:" + strPath);
                return null;
            }
            item.spr = spr;

            itemList.Add(item);
            mResDict[strPath] = itemList;
        }

        return item.spr;
    }

    public void ReleaseObject(string strPath, int objID, bool forceRelease = false)
    {
        ReleaseItem(strPath, objID, forceRelease);
    }

    public void ReleaseSprite(string strPath, bool forceRelease = false)
    {
        ReleaseItem(strPath, 0, forceRelease);
    }

    void ReleaseItem(string strPath, int objID, bool forceRelease)
    {
        List<AssetItem> itemList = null;
        AssetItem item = null;
        if (mResDict.TryGetValue(strPath, out itemList))
        {
            //sprite
            if (objID == 0)
            {
                if (itemList.Count > 0)
                {
                    item = itemList[0];
                    if (item.refCount > 0)
                    {
                        item.refCount--;
                    }
                }
            }
            else
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    if (itemList[i].objID == objID)
                    {
                        item = itemList[i];
                        item.refCount = 0;
                        break;
                    }
                }
            }
            if (item == null)
            {
                return;
            }

            if (item.refCount == 0 && forceRelease)
            {
                if (objID == 0)
                {
                    if (item.spr != null)
                    {
                        Sprite.Destroy(item.spr);
                        item.spr = null;
                    }

                    Resources.UnloadAsset(item.obj);
                    itemList.Remove(item);
                    item.obj = null;
                    mResDict.Remove(strPath);
                }
                else
                {
                    
                    Object.Destroy(item.obj);
                    itemList.Remove(item);
                    item.obj = null;
                    if (itemList.Count == 0)
                    {
                        mResDict.Remove(strPath);
                    }

                }
            }
            else if (item.refCount == 0)
            {
                if (objID != 0)
                {
                    GameObject go = item.obj as GameObject;
                    if (go)
                    {
//                        go.transform.localScale = Vector3.zero;
                        go.SetActive(false);
                    }
                }
                else
                {
                }
            }
        }
    }

    public void ReleaseAll()
    {
        foreach (List<AssetItem> itemList in mResDict.Values)
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Object.Destroy(itemList[i].obj);
                itemList[i].obj = null;
            }
        }
        mResDict.Clear();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void Test()
    {
        Object go = Resources.Load("Prefab/testobj");
        GameObject go2 = Object.Instantiate(go) as GameObject;
        int id1 = go2.GetInstanceID();

        GameObject go3 = Object.Instantiate(go) as GameObject;
        int id2 = go3.GetInstanceID();
        if (id1 != id2)
        {
            Debug.LogError("ResMgr id1:" + id1.ToString() + ",id2:" + id2.ToString());
        }
    }
}
