#include "LevelReadWrite.h"

struct V3 {
	float x, y, z;
};

extern "C" {
	//function to load the level data into c++ from a file, will have to use other functions to access it
	void PLUGIN_API LoadLevel(char* fileName) {
		std::string name = std::string(fileName);
		LevelReadWrite::ReadLevelData(name);
	}

	//function to save the level data to a file from the c++ data, will have to use functions to get the data into the c++ code
	void PLUGIN_API SaveLevel(char* fileName) {
		std::string name = std::string(fileName);
		LevelReadWrite::WriteLevelData(name);
	}

	//clear all of the locally stored c++ data
	void PLUGIN_API ClearData() {
		LevelReadWrite::ClearSavedData();
	}

	//get the number of enemies in the level
	int PLUGIN_API GetNumOfEnemies() {
		return LevelReadWrite::GetNumOfEnemies();
	}

	//function to get the enemy's position based on index - will iterate over them all in C# 
	V3 PLUGIN_API GetEnemyPosFromIndex(int index) {
		SerializableEntities temp = LevelReadWrite::GetEnemyAtIndex(index);
		V3 newPos;
		newPos.x = temp.x;
		newPos.y = temp.y;
		newPos.z = temp.z;

		return newPos;
	}

	//function to add an enemy to the level
	void PLUGIN_API AddEnemyToLevel(float x, float y, float z) {
		SerializableEntities newEnemy;
		newEnemy.x = x;
		newEnemy.y = y;
		newEnemy.z = z;
		newEnemy.type = 0; //type 0 is enemy, type 1 is platforms

		LevelReadWrite::AddEnemy(newEnemy);
	}

	//get the number of platforms in the level
	int PLUGIN_API GetNumOfPlatforms() {
		return LevelReadWrite::GetNumOfPlatforms();
	}

	//function to get the platform's position based on index - will iterate over them all in C# 
	V3 PLUGIN_API GetPlatformPosFromIndex(int index) {
		SerializableEntities temp = LevelReadWrite::GetPlatformAtIndex(index);
		V3 newPos;
		newPos.x = temp.x;
		newPos.y = temp.y;
		newPos.z = temp.z;

		return newPos;
	}

	//function to add a platform to the level
	void PLUGIN_API AddPlatformToLevel(float x, float y, float z) {
		SerializableEntities newPlatform;
		newPlatform.x = x;
		newPlatform.y = y;
		newPlatform.z = z;
		newPlatform.type = 1; //type 0 is enemy, type 1 is platforms

		LevelReadWrite::AddPlatfrom(newPlatform);
	}
}