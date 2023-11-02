#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <filesystem> // Include the <filesystem> header

bool compareCSVFiles(const std::string &folder1, const std::string &folder2)
{
    // Get a list of CSV files in both directories
    std::vector<std::string> csvFiles1;
    std::vector<std::string> csvFiles2;

    // List CSV files in folder1
    for (const auto &entry : std::filesystem::directory_iterator(folder1))
    {
        if (entry.path().extension() == ".csv")
        {
            csvFiles1.push_back(entry.path().filename().string());
        }
    }

    // List CSV files in folder2
    for (const auto &entry : std::filesystem::directory_iterator(folder2))
    {
        if (entry.path().extension() == ".csv")
        {
            csvFiles2.push_back(entry.path().filename().string());
        }
    }

    // Compare the contents of corresponding files
    for (const auto &file1 : csvFiles1)
    {
        std::string path1 = folder1 + "/" + file1;
        std::string file2 = file1;
        std::string path2 = folder2 + "/" + file2;

        // Open and compare files
        std::ifstream fileStream1(path1);
        std::ifstream fileStream2(path2);

        if (!fileStream1.is_open() || !fileStream2.is_open())
        {
            std::cerr << "Failed to open files for comparison." << std::endl;
            return false;
        }

        std::string line1, line2;

        // Compare lines in the two files
        while (std::getline(fileStream1, line1) && std::getline(fileStream2, line2))
        {
            if (line1 != line2)
            {
                std::cout << "File " << file1 << " and " << file2 << " have differences." << std::endl;
                return false;
            }
        }

        if (fileStream1.eof() && fileStream2.eof())
        {
            std::cout << "File " << file1 << " and " << file2 << " are identical." << std::endl;
        }
        else
        {
            std::cout << "File " << file1 << " and " << file2 << " have differences." << std::endl;
        }

        fileStream1.close();
        fileStream2.close();
    }

    return true;
}

int main()
{
    std::string folder1 = "AWSBillingEngine/AWSBillingEngine2/Bills";
    std::string folder2 = "AWSBillingEngine/AWSBillingEngine2/Output";

    if (compareCSVFiles(folder1, folder2))
    {
        std::cout << "All corresponding CSV files are identical." << std::endl;
    }
    else
    {
        std::cout << "Differences found in corresponding CSV files." << std::endl;
    }

    return 0;
}
