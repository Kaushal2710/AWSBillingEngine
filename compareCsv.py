import os
import pandas as pd

# Specify the directories containing the CSV files
folder1 = 'AWSBillingEngine/AWSBillingEngine2/Bills'
folder2 = 'AWSBillingEngine/AWSBillingEngine2/Output'

# Get a list of CSV files in both directories
csv_files1 = [f for f in os.listdir(folder1) if f.endswith('.csv')]
csv_files2 = [f for f in os.listdir(folder2) if f.endswith('.csv')]

# Compare the contents of corresponding files
for file1, file2 in zip(csv_files1, csv_files2):
    path1 = os.path.join(folder1, file1)
    path2 = os.path.join(folder2, file2)

    df1 = pd.read_csv(path1)
    df2 = pd.read_csv(path2)

    # Compare dataframes
    if df1.equals(df2):
        print(f"File {file1} and {file2} are identical.")
    else:
        print(f"File {file1} and {file2} have differences.")

