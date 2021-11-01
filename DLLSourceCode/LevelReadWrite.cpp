#include "LevelReadWrite.h"

std::vector<SerializableEntities> LevelReadWrite::enemies = std::vector<SerializableEntities>();
std::vector<SerializableEntities> LevelReadWrite::platforms = std::vector<SerializableEntities>();

int LevelReadWrite::GetNumOfEnemies()
{
	return enemies.size();
}

int LevelReadWrite::GetNumOfPlatforms()
{
	return platforms.size();
}

SerializableEntities LevelReadWrite::GetEnemyAtIndex(int index)
{
	if (index < enemies.size()) return enemies[index];

	return SerializableEntities();
}

SerializableEntities LevelReadWrite::GetPlatformAtIndex(int index)
{
	if (index < platforms.size()) return platforms[index];

	return SerializableEntities();
}

void LevelReadWrite::AddEnemy(SerializableEntities enemy)
{
	enemies.push_back(enemy);
}

void LevelReadWrite::AddPlatfrom(SerializableEntities platform)
{
	platforms.push_back(platform);
}

void LevelReadWrite::ClearSavedData()
{
	enemies.clear();
	platforms.clear();
}

void LevelReadWrite::ReadLevelData(std::string fileName)
{
	//read the file
	std::ifstream file;
	file.open(fileName, std::ios::in | std::ios::binary);

	if (file.is_open()) {
		std::cout << "File opened for reading";
		//read the file piece by piece
		SerializableEntities temp;
		while (file.read(reinterpret_cast<char*>(&temp), sizeof(temp))) {
			//if it's an enemy store it in the enemies array
			if (temp.type == 0) enemies.push_back(temp);
			//otherwise it must be a platform so store it in the platform array
			else if (temp.type == 1) platforms.push_back(temp);
		}
	}
	else {
		std::cout << "File failed to open";
	}
}

void LevelReadWrite::WriteLevelData(std::string fileName)
{
	//make the file
	std::ofstream file;
	file.open(fileName, std::ios::out | std::ios::binary);

	if (file.is_open()) {
		std::cout << "File opened for writting";
		//write all of the enemies in
		file.write(reinterpret_cast<char*>(&enemies[0]), enemies.size() * sizeof(SerializableEntities));

		//write all of the platforms in
		file.write(reinterpret_cast<char*>(&platforms[0]), platforms.size() * sizeof(SerializableEntities));

		//close the file
		file.close();
	}
	else {
		std::cout << "File failed to open";
	}
}