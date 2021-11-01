#pragma once
#include <string>
#include <fstream>
#include <iostream>
#include <vector>

#define PLUGIN_API __declspec(dllexport)

struct SerializableEntities {
	float x, y, z;
	int type;
};

static class PLUGIN_API LevelReadWrite {
public:
	//functions to get the number of enemies and platforms
	static int GetNumOfEnemies();
	static int GetNumOfPlatforms();

	//functions to get specific enemies and platforms
	static SerializableEntities GetEnemyAtIndex(int index);
	static SerializableEntities GetPlatformAtIndex(int index);

	//functions to add enemies and platforms
	static void AddEnemy(SerializableEntities enemy);
	static void AddPlatfrom(SerializableEntities platform);

	//clear vector
	static void ClearSavedData();

	//read from file
	static void ReadLevelData(std::string fileName);

	//write to file
	static void WriteLevelData(std::string fileName);

private:
	//vectors with the details for both enemies and platforms
	static std::vector<SerializableEntities> enemies;
	static std::vector<SerializableEntities> platforms;
};