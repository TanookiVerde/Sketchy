using UnityEngine;

public class ImageComparator{
    /*
    * Retorna um float de 0 a 1, que representa a porcentagem de semelhança (0 a 100%)
    */
    public static float CheckSimilarity(Texture2D newImage, Texture2D originalImage) {
        Color strokeColor = Color.black;

        float similarity = 0,
              strokePixels = 0;

        int canvasWidth = originalImage.width, 
            canvasHeight = originalImage.height;

        /*
        * Cria uma textura com traços mais grossos da imagem original. 
        * originalImage nunca é de fato comparada com a newImage, quem tem esta função é a auxImage 
        */
        Texture2D auxImage = CreateTextureMap(originalImage, strokeColor);

        for(int y = 0; y < canvasHeight; y++)
        {
            for(int x = 0; x < canvasWidth; x++)
            {
                //pega a quantidade pixels pretos que a imagem original possui
                if (auxImage.GetPixel(x, y) == strokeColor) strokePixels++;
                
                //verifica somente os traços do novo desenho em busca de similaridades ou diferenças
                if (newImage.GetPixel(x, y) == strokeColor)
                {
                    if (auxImage.GetPixel(x, y) == newImage.GetPixel(x, y)) {
                        similarity+=1;
                    } else {
                        similarity-=.4f;
                    }
                }
            }
        }

        //Debug.Log("similarity: " + similarity);
        //Debug.Log("strokePixels: " + strokePixels);

        /*
        * Busca a porcentagem de similaridade entre os strokes, e não na imagem toda
        * Isso é necessário para que o sistema atual de texture map funcione 
        */
        float result = (similarity / strokePixels);
        //Debug.Log("result: " + result);
        return result;
    }

    public static Texture2D CreateTextureMap(Texture2D originalImage, Color strokeColor) {
        Texture2D handicapMap = new Texture2D(originalImage.width, originalImage.height);

        /*
        * Roda por toda a imagem procurando pixels pretos
        * Ao achar um, desenha um stroke de tamanho "diameter" em cima dele
        */
        for (int y = 0; y < handicapMap.height; y++)
        {
            for (int x = 0; x < handicapMap.width; x++)
            {
                if(originalImage.GetPixel(x,y) == strokeColor) {

                    //basicamente PaintWithBrush()
                    int diameter = 14;
                    for (int i = 0; i < diameter; i++) {
                        for (int j = 0; j < diameter; j++) {
                            handicapMap.SetPixel(x - (diameter / 2) + i, y - (diameter / 2) + j, strokeColor /*Color.red*/);
                        }
                    }

                }
            }
        }
        handicapMap.Apply();

        return handicapMap;
    }

}
