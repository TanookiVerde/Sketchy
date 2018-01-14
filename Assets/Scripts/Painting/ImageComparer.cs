using UnityEngine;

public class ImageComparer{

    //retorna um float de 0 a 1, que representa a porcentagem de semelhança (0 a 100%)
    public static float CheckSimilarity(Texture2D newImage, Texture2D originalImage) {
        Color strokeColor = Color.black;

        int similarity = 0,
            strokePixels = 0;

        int canvasWidth = originalImage.width, 
            canvasHeight = originalImage.height;

        // <<< montar imagem de análise aqui e substituir linha de baixo
        Texture2D auxImage = originalImage;

        for(int y = 0; y < canvasHeight; y++)
        {
            for(int x = 0; x < canvasWidth; x++)
            {
                if (originalImage.GetPixel(x, y) == strokeColor) strokePixels++;
                
                if (newImage.GetPixel(x, y) == strokeColor)
                {
                    if (originalImage.GetPixel(x, y) == newImage.GetPixel(x, y)) {
                        similarity++;
                    } else {
                        similarity--;
                    }
                }
            }
        }

        //Debug.Log("similarity: " + similarity);
        //Debug.Log("strokePixels: " + strokePixels);
        float result = ((float)similarity / strokePixels);
        return result;
    }

}
