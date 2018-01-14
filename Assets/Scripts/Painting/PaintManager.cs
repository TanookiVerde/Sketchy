﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour {
	[Header("Texture Preferences")]
	[SerializeField] private Renderer paintArea;								//Mesh Renderer do plano sendo utilizado
	[SerializeField] private int[] canvasSize = new int[2];						//Tamanho em [height,width] da textura
	private Texture2D texture;													//Textura que iremos criar e editar

	[Header("Colors")]
	[SerializeField] private List<Color> colors;
	[SerializeField] private Color currentColor = Color.black;

	[Header("Brush Sizes")]
	[SerializeField] private BrushSize brush;
	[SerializeField] private int[,] smallBrush = {{0,0,0,0,0,0,0},
												  {0,0,0,0,0,0,0},
												  {0,0,0,1,0,0,0},
												  {0,0,1,1,1,0,0},
												  {0,0,0,1,0,0,0},
												  {0,0,0,0,0,0,0},
												  {0,0,0,0,0,0,0}};
	[SerializeField] private int[,] mediumBrush = {{0,0,0,0,0,0,0},
												   {0,0,1,1,1,0,0},
												   {0,1,1,1,1,1,0},
												   {0,1,1,1,1,1,0},
												   {0,1,1,1,1,1,0},
												   {0,0,1,1,1,0,0},
												   {0,0,0,0,0,0,0}};
	[SerializeField] private int[,] bigBrush = {{0,0,1,1,1,0,0},
												{0,1,1,1,1,1,0},
												{1,1,1,1,1,1,1},
												{1,1,1,1,1,1,1},
												{1,1,1,1,1,1,1},
												{0,1,1,1,1,1,0},
												{0,0,1,1,1,0,0}};

	private void Start(){
		Application.targetFrameRate = 300;
		InitializeTexture(canvasSize);
		AttachTexture();
		ClearTexture(Color.white);
	}
	private void Update(){
		if(texture != null) PaintLoop();
		if(Input.GetKey(KeyCode.C)) ClearTexture(Color.white);
	}
	private void InitializeTexture(int[] size, FilterMode mode = FilterMode.Point){
		/*
		Cria uma textura com o tamanho desejado e diz qual sei filtermode. Filtermode.Point garante uma imagem pixelada.
		*/
		texture = new Texture2D(size[0],size[1]);
		texture.filterMode = mode;
	}
	private void AttachTexture(){
		/*
		Coloca a textura criada como principal no plano.
		*/
		paintArea.sharedMaterial.mainTexture = texture;
	}
	private void PaintLoop(){
		/*
		Eh chamada uma vez for Update(). 
		Dentro dela a posicao do mouse eh calculada e, se MouseButton(0) estiver pressionado, pinta a textura com um certo tamanho de pincel e uma cor em uma posicao.
		*/
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if(Input.GetMouseButton(0)){
			if(Physics.Raycast(ray, out hit,Mathf.Infinity)){
				Vector2 pixelCoordinate = CoordinateFromRaycastHit(hit);
				Paint(texture,pixelCoordinate,currentColor,brush);
			}
		}
	}
	private void Paint(Texture2D texture, Vector2 position, Color color,BrushSize brush = BrushSize.SMALL){
		/*
		Dado uma textura, uma posicao, uma cor e um tipo de pincel, esta funcao chama PaintWithBrush com parametros dependendo do tamanho do pincel.
		*/
		switch(brush){
			case BrushSize.SMALL:
				PaintWithBrush(smallBrush, (int) position.x, (int)position.y, color);
				break;
			case BrushSize.MEDIUM:
				PaintWithBrush(mediumBrush, (int) position.x, (int)position.y, color);
				break;
			case BrushSize.BIG:
				PaintWithBrush(bigBrush, (int) position.x, (int)position.y, color);
				break;
		}
	}
	private void PaintWithBrush(int[,] brushMap,int x, int y, Color color){
		/*
		Funcao que de fato pinta a textura usando a matriz brushMap como referencia para os pixels ao redor do centro.
		*/
		for(int i = 0; i < 7; i++){
			for(int j = 0; j < 7; j++){
				if(brushMap[i,j] == 1) texture.SetPixel(x - 7 + i, y - 7 + j, color);
			}
		}
		texture.Apply();
	}
	private void ClearTexture(Color c) {
		/*
		Percorre todos os pixels da textura e os pinta de uma cor c.
		*/
		for (int i = 0; i < canvasSize[1]; i++) {
			for (int j = 0; j < canvasSize[0]; j++) {
				texture.SetPixel(i,j, c);
			}
		}
		texture.Apply();
	}
	private Vector2 CoordinateFromRaycastHit(RaycastHit hit){
		/*
		Calcula a partir da colisão com o plano a coordenada do pixel selecionado.
		PS1.: alguns ajustes de sinal foram necessarios devido a orientacao do plano.
		PS2.: algoritmo retirado e adaptado do codigo do Vinicius Garcia.
		*/
		Vector2 uv;
		uv.x = -(hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
		uv.x *= canvasSize[0];
		uv.y = -(hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;
		uv.y *= canvasSize[1];
		return uv;
	}
}
public enum BrushSize{
	SMALL,MEDIUM,BIG
}