using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TestShakespeare : MonoBehaviour
{
	[Header("Genetic Algorithm")]
	[SerializeField] int populationSize = 200;
	[SerializeField] float mutationRate = 0.01f;
	[SerializeField] int elitism = 5;

	[Header("Level Info")]
	[SerializeField] int MaxBallCount;

	[Header("Other Info")]
	public GameObject Board;

	[Header("Auto Player")]
	public GameObject AIPlayer;

	private GeneticAlgorithm<Location> ga;
	private System.Random random;

	private int dnaSize;

	void Start()
	{
		random = new System.Random();
		ga = new GeneticAlgorithm<Location>(populationSize, MaxBallCount, random, GetRandomLocation, FitnessFunction, elitism, mutationRate);
	}

	void Update()
	{
		ga.NewGeneration();

		if (ga.BestFitness == 1)
		{
			this.enabled = false;
		}
	}

	private Location GetRandomLocation()
	{
		int width = (int)Board.transform.localScale.x / 2;
		int height = (int)Board.transform.localScale.y / 2;
		float x = random.Next(-width, width) % 0.3f;
		float y = random.Next(-height, height) % 0.3f;

		Location loc = new Location(x, y);

		return loc;
	}

	private float FitnessFunction(int index)
	{
		float score = 0;
		DNA<Location> dna = ga.Population[index];

		CreateNewLevel(dna);
		GameManager.instance.newLevel(1);

		autoPlay(); // ai 플레이 후 결과 확인

		return score;
	}

	private void CreateNewLevel(DNA<Location> dna)
    {
		List<Location> tmp = new List<Location>();
		foreach(Location loc in dna.Genes)
		{
			tmp.Add(loc);
		}

		Level lev = new Level(tmp);

		GameManager.instance.saveData(lev, 1);
	}


	private void autoPlay()
    {
		GameManager.instance.isPlayerTurn = true;
		AIPlayer.SetActive(true);
    }
}