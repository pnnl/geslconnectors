% this script shows how to use MATLAB SigLab API functions
clear;
clc;

cfg = config();
email = cfg.email;
apikey = cfg.apikey;
url = 'https://gesl.ornl.gov/api/apps/gesl';

% Get all tags
alltags = get_event_tags(email, apikey, url);


% Find tag IDs
tagsToFind = {'Generator', 'Transmission', 'On', 'Arc Furnace'};
requiredAncestorTags = {'Equipment'};
foundTags = find_ids_by_tags(alltags, tagsToFind, requiredAncestorTags);


% Displaying the results and extacting IDs
allIds = [];
disp('Results for find_ids_by_tags:');
for i = 1:length(tagsToFind)
    tag = tagsToFind{i};
     % Convert the tag to a valid MATLAB field name
    validFieldName = matlab.lang.makeValidName(tag);
    fprintf('Tag: %s, IDs: ', tag);
    if isfield(foundTags, validFieldName) && ~isempty(foundTags.(validFieldName))
        disp(foundTags.(validFieldName)); 
        allIds = [allIds, foundTags.(validFieldName)];        
    else
        fprintf('None\n\n');         
    end
end


event_start = '2017-01-01 00:00:00';
event_end = '2017-02-28 23:23:59';

data_source = ['Provider 9'];
event_tag_ids = allIds;

% Get event signuture IDs
event_sig_ids = get_event_ids(email, apikey, url, 'event_start', event_start, 'event_end', event_end,   'event_tag_ids', event_tag_ids, 'data_source', data_source);
disp(event_sig_ids);


saveZipPath = fullfile(pwd, 'zip');  % Directory for saving zip files
% Ensure the save directory exists
if ~exist(saveZipPath, 'dir')
    mkdir(saveZipPath);
end

% Loop over each signature ID and get zip files
for i = 1:length(event_sig_ids)
    get_event_data(event_sig_ids(i), email, apikey, url, saveZipPath);
end

% extract csv files
csvDir = fullfile(pwd, 'csv');

% Create the 'csv' directory if it doesn't exist
if ~exist(csvDir, 'dir')
    mkdir(csvDir);
end

% Get a list of all zip files in the 'zip' directory
zipFiles = dir(fullfile(saveZipPath, '*.zip'));
% Loop through each zip file and unzip it to the 'csv' directory
for i = 1:length(zipFiles)
    % Full path to the zip file
    zipFilePath = fullfile(saveZipPath, zipFiles(i).name);   
    % Unzip the file into the 'csv' directory
    unzip(zipFilePath, csvDir);
    disp(['Unzipped ' zipFiles(i).name ' to ' csvDir]);
end


%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

% List all the CSV files in the directory
csvFiles = dir(fullfile(csvDir, '*.csv'));

% Create a structure to hold the data
dataStruct = struct;

% Iterate over the list of CSV files
for i = 1:length(csvFiles)
    csv_filename = csvFiles(i).name;
    csv_path = fullfile(csvDir, csv_filename);
    
    
    % Replace hyphens with '' in the filename
    validFieldName = strrep(csv_filename, '-', '');
    % Remove '.csv' extension and read the file into a MATLAB table
    validFieldName = erase(validFieldName, '.csv');
    
    % Read the CSV file into a MATLAB table
    dataStruct.(validFieldName) = readtable(csv_path);
    fprintf('Loaded %s into table.\n', csv_filename);
end

% Plotting
figure('Position', [100, 100, 1000, 600]); 
hold on; % Hold on to plot multiple lines

% Iterate over the data to plot
fields = fieldnames(dataStruct);
for i = 1:length(fields)
    filename = fields{i};
    df = dataStruct.(filename);
    
    % Check if 'Time' and 'P001.f' columns exist
    if any(strcmp('Time', df.Properties.VariableNames)) && ...
       any(strcmp('P001_f', df.Properties.VariableNames))
        plot(df.Time, df.('P001_f'), 'DisplayName', filename);
    end
end

xlabel('Time');
ylabel('Frequency, Hz');
title('Frequency Plot');
legend('show');
grid on;



