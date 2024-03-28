% this function returns signuture IDs for specific filter parameters
function result = get_event_ids(email, api_key, url, varargin)

    % Validate email and api_key
    validation_msg = validate_api_input(email, api_key);
    if ~isempty(validation_msg)
        result = validation_msg;
        return;
    end

    % Parse optional parameters
    p = inputParser;
    addOptional(p, 'event_type_ids', []);
    addOptional(p, 'event_tag_ids', []);
    addOptional(p, 'data_source', []);
    addOptional(p, 'data_site', []);
    addOptional(p, 'data_sensor', []);
    addOptional(p, 'event_type', '');
    addOptional(p, 'event_start', '');
    addOptional(p, 'event_end', '');
    parse(p, varargin{:});

    % Check for at least one filter parameter
    if isempty(p.Results.event_type_ids) && isempty(p.Results.event_tag_ids) && ...
            isempty(p.Results.data_source) && isempty(p.Results.event_type) && ...
            isempty(p.Results.event_start) && isempty(p.Results.event_end)
        result = 'Invalid Criteria, No Filter Parameters Provided';
        return;
    end

    % Validate date format if provided
    if ~isempty(p.Results.event_start) || ~isempty(p.Results.event_end)
        try
            datetime(p.Results.event_start, 'InputFormat', 'yyyy-MM-dd HH:mm:ss');
            datetime(p.Results.event_end, 'InputFormat', 'yyyy-MM-dd HH:mm:ss');
        catch
            result = 'Invalid date range or incorrect data format, should be YYYY-MM-DD HH:mm:ss';
            return;
        end
    end

    % Construct parameters for the request
    params = struct(...
        'email', email, ...
        'apikey', api_key, ...
        'eventtagid', {num2cell(p.Results.event_tag_ids)}, ...       
        'eventstart', p.Results.event_start, ...
        'eventend', p.Results.event_end, ...             
        'output', 'sigids');
    %'eventtypeid', {num2cell(p.Results.event_type_ids)}, ...

        % 'datasource', {cellstr(p.Results.data_source)}, ... % Assuming this is a string or cell array
        %'eventtype', p.Results.event_type, ...
       
       
    if ~isempty(p.Results.event_type_ids)
        params.eventtypeid = {cellstr(p.Results.event_type_ids)};
    end
    
    if ~isempty(p.Results.data_source)
        params.datasource = cellstr(p.Results.data_source);
    end
    
    disp(params); 
    %if ~isempty(p.Results.data_sensor)
    %    params.sensor = wrapInCell(p.Results.data_sensor);
    %end

    % Call the request_data function (assuming it's implemented)
    result = request_data(params, url);
end

%function cellArray = wrapInCell(value)
%    if isempty(value)
%        cellArray = {};
%    elseif iscell(value)
%        cellArray = value;
%    else
%        cellArray = {value};
%    end
%end

