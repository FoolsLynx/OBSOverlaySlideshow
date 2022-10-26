using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlideshow : MonoBehaviour
{
    public RawImage image;
    public RectTransform imageContainer;

    private AspectRatioFitter aspectRatioFitter;

    [Header("Controller")]
    public bool showController = false;
    public GameObject controllerPanel;

    public Slider speedSlider;
    public TextMeshProUGUI speedValue;

    public Toggle randomToggle;

    public Slider xSlider;
    public TextMeshProUGUI xValue;

    public Slider ySlider;
    public TextMeshProUGUI yValue;

    public Slider widthSlider;
    public TextMeshProUGUI widthValue;

    public Slider heightSlider;
    public TextMeshProUGUI heightValue;

    [Min(1f)]
    public float imageDisplayTime = 5f;

    [Header("Randomiser")]
    public bool randomStartIndex = true;
    public bool randomImage = true;

    private int currentIndex;
    private int maxIndex;

    private bool waiting = false;
    private bool pause = false;

    

    private void Awake()
    {
        aspectRatioFitter = image.GetComponent<AspectRatioFitter>();
    }

    private void Start()
    {
        maxIndex = ImageLoader.instance.GetImageCount();
        if(maxIndex == 0)
        {
            Debug.LogError("Max Index was 0");
            Destroy(this);
        }
        currentIndex = randomStartIndex ? Random.Range(0, maxIndex) : 0;

        Texture2D tex = ImageLoader.instance.GetImage(currentIndex);
        if (tex == null)
        {
            Debug.Log("Texture was null on start");
            Destroy(this);
            return;
        }
        image.texture = tex;

        int width = tex.width;
        int height = tex.height;
        float aspect = (float)width / (float)height;
        aspectRatioFitter.aspectRatio = aspect;
        controllerPanel.SetActive(showController);


        speedSlider.value = imageDisplayTime;
        speedValue.text = speedSlider.value.ToString();

        randomToggle.isOn = randomImage;

        xSlider.value = imageContainer.anchoredPosition.x;
        xValue.text = xSlider.value.ToString();

        ySlider.value = imageContainer.anchoredPosition.y;
        yValue.text = (-ySlider.value).ToString();

        widthSlider.value = imageContainer.sizeDelta.x;
        widthValue.text = widthSlider.value.ToString();

        heightSlider.value = imageContainer.sizeDelta.y;
        heightValue.text = heightSlider.value.ToString();

        LoadSettings();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            ShowHideController();
        }
        if(Input.GetKeyDown(KeyCode.F5))
        {
            SaveSettings();
            return;
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadSettings();
            return;
        }
        if (pause) return;
        if(!waiting)
        {
            StartCoroutine(ChangeImage());
        }
    }


    private IEnumerator ChangeImage()
    {
        waiting = true;
        yield return new WaitForSeconds(imageDisplayTime);
        SetNextImage();
        waiting = false;
    }

    private void SetNextImage()
    {
        if (maxIndex <= 1) return;
        if(!randomImage)
        {
            currentIndex++;
            if(currentIndex >= maxIndex)
            {
                currentIndex = 0;
            }
        } else
        {
            // Get Random Number
            int r = Random.Range(0, maxIndex);
            // Get Old Index
            int oldIndex = currentIndex;
            // Set Current Index
            currentIndex = r;
            while(currentIndex == oldIndex)
            {
                r = Random.Range(0, maxIndex);
                currentIndex = r;
            }

        }
        Texture2D tex = ImageLoader.instance.GetImage(currentIndex);
        if(tex == null)
        {
            Debug.Log("Texture was null");
            Destroy(this);
            return;
        }
        int width = tex.width;
        int height = tex.height;
        float aspect = (float)width / (float)height;

        aspectRatioFitter.aspectRatio = aspect;
        image.texture = tex;

    }

    private void ShowHideController()
    {
        showController = !showController;
        controllerPanel.SetActive(showController);
    }

    
    public void OnSpeedSliderChanged()
    {
        speedValue.text = speedSlider.value.ToString();
        imageDisplayTime = speedSlider.value;
    }

    public void OnRandomToggleChanged()
    {
        randomImage = randomToggle.isOn;
    }

    public void OnXPositionChanged()
    {
        float x = xSlider.value;
        xValue.text = xSlider.value.ToString();

        Vector2 newPosition = imageContainer.anchoredPosition;
        newPosition.x = x;
        imageContainer.anchoredPosition = newPosition;
        
    }

    public void OnYPositionChanged()
    {
        float y = ySlider.value;
        yValue.text = ySlider.value.ToString();

        Vector2 newPosition = imageContainer.anchoredPosition;
        newPosition.y = -y;
        imageContainer.anchoredPosition = newPosition;
    }

    public void OnWidthChanged()
    {
        float w = widthSlider.value;
        widthValue.text = widthSlider.value.ToString();

        Vector2 newSize = imageContainer.sizeDelta;
        newSize.x = w;
        imageContainer.sizeDelta = newSize;
        FixAspectRatio(newSize);
    }

    public void OnHeightChanged()
    {
        float h = heightSlider.value;
        heightValue.text = heightSlider.value.ToString();

        Vector2 newSize = imageContainer.sizeDelta;
        newSize.y = h;
        imageContainer.sizeDelta = newSize;
        FixAspectRatio(newSize);
    }

    private void FixAspectRatio(Vector2 newSize)
    {
        if (newSize.x > newSize.y)
        {
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        }
        else if (newSize.x < newSize.y)
        {
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        }
        image.rectTransform.sizeDelta = Vector2.zero;
        Texture tex = image.texture;
        int width = tex.width;
        int height = tex.height;
        float aspect = (float)width / (float)height;

        aspectRatioFitter.aspectRatio = aspect;

    }

    public void SaveSettings()
    {
        ImageSettings settings = new()
        {
            xPosition = (int)xSlider.value,
            yPosition = (int)ySlider.value,
            minWidth = (int)widthSlider.value,
            minHeight = (int)heightSlider.value,

            showController = showController,

            displayTime = speedSlider.value,
            randomised = randomToggle.isOn
            
        };

        string json = JsonUtility.ToJson(settings, true);

        // Get Executable Folder
        string path = Application.dataPath;
        path = Path.GetFullPath(Path.Combine(path, @"..\"));
        path = Path.Combine(path, "Config.json");

        if(File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, json);
    }

    public void LoadSettings()
    {
        string path = Application.dataPath;
        path = Path.GetFullPath(Path.Combine(path, @"..\"));
        path = Path.Combine(path, "Config.json");
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        ImageSettings settings = JsonUtility.FromJson<ImageSettings>(json);

        xSlider.value = settings.xPosition;
        ySlider.value = settings.yPosition;
        widthSlider.value = settings.minWidth;
        heightSlider.value = settings.minHeight;

        showController = settings.showController;

        speedSlider.value = settings.displayTime;
        randomImage = settings.randomised;
        randomStartIndex = settings.randomised;


        xValue.text = xSlider.value.ToString();
        yValue.text = ySlider.value.ToString();
        widthValue.text = widthSlider.value.ToString();
        heightValue.text = heightSlider.value.ToString();

        speedValue.text = speedSlider.value.ToString();

        randomToggle.isOn = randomImage;

        controllerPanel.SetActive(showController);
    }
}
