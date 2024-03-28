% this function dounloads zip file for e specific signuture ID
function get_event_data(SigID, email, apikey, url, savePath)
    % Base data structure
    baseData = struct(...
        'email', email, ...
        'apikey', apikey, ...
        'filetype', {{'raw', 'scrubbed', 'demod', 'demod fft'}}, ...
        'output', 'data');

    % Set the web options, including header fields
    options = weboptions('MediaType', 'application/json', 'RequestMethod', 'post', 'Timeout', Inf);
    options.CertificateFilename = ('');

    % Update the data payload with the current signature ID
    data = baseData;
    data.sigid = SigID;

    % Send a POST request for the current signature ID
    zipfile = webwrite(url, data, options);

    % Define the filename for the zip file
    zipFileName = fullfile(savePath, sprintf('sigid_%d.zip', SigID)); 

    % Open a file for writing in binary mode
    fid = fopen(zipFileName, 'wb');
    if fid == -1
        error('Cannot open file for writing: %s', zipFileName);
    end

    % Write the data to the file
    fwrite(fid, zipfile, 'uint8');

    % Close the file
    fclose(fid);

    disp(['File saved to: ' zipFileName]);
end
