using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    public static ImageLoader instance;

    private static readonly List<string> ImageExtensions = new 
        List<string> { ".png", ".jpg", ".jpeg", ".gif" };

    private string imagePath;
    [SerializeField] private List<Texture2D> images;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Get Executable Folder
        string path = Application.dataPath;
        path = Path.GetFullPath(Path.Combine(path, @"..\"));

        // Get Slideshow Image Folder
        path = Path.Combine(path, @"Slideshow Images\");
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        // Set the Path
        imagePath = path;

        LoadImages();
    }

    private void LoadImages()
    {
        images = new();
        string[] files = Directory.GetFiles(imagePath);
        foreach(string file in files)
        {
            if (!IsValidImageFile(file)) continue;

            if (IsAnimatedImageFile(file))
            {
                //SetupAnimatedImage(file);
            }
            else
            {
                var bytes = File.ReadAllBytes(file);
                Texture2D tex = new(2, 2);
                tex.LoadImage(bytes);
                images.Add(tex);
            }
        }
        LoadPixelArtImages();
    }

    private void LoadPixelArtImages()
    {
        string path2 = imagePath;
        path2 = Path.Combine(path2, @"Pixel Art\");
        if(!Directory.Exists(path2))
        {
            Directory.CreateDirectory(path2);
        }
        string[] files = Directory.GetFiles(path2);
        foreach (string file in files)
        {
            if (!IsValidImageFile(file)) continue;

            if (IsAnimatedImageFile(file))
            {
                //SetupAnimatedImage(file, true);
            }
            else
            {
                var bytes = File.ReadAllBytes(file);
                Texture2D tex = new(2, 2);
                tex.LoadImage(bytes);
                tex.filterMode = FilterMode.Point;
                images.Add(tex);
            }
        }
    }

    private void SetupAnimatedImage(string file, bool point = false)
    {
        try
        {
            Debug.Log(file);
        } catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    

    private bool IsValidImageFile(string file)
    {
        string ext = Path.GetExtension(file).ToLower();
        return ImageExtensions.Contains(ext);
    }

    private bool IsAnimatedImageFile(string file)
    {
        string ext = Path.GetExtension(file).ToLower();
        return ext.Equals(".gif");
    }

    public int GetImageCount()
    {
        return images.Count;
    }

    public Texture2D GetImage(int index)
    {
        if (images.Count == 0) return null;
        if(index >= images.Count)
        {
            index = 0;
        }
        return images[index];
    }

    public bool IsAnimated(Texture2D texture)
    {
        return false;
    }
}
