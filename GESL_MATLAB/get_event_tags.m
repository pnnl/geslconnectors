% this function returns ALL EVENT TAGS 
function result = get_event_tags(email, api_key, url)
    validation_result = validate_api_input(email, api_key);
    if ~isempty(validation_result)
        result = validation_result;
        return;
    end

    params = struct('email', email, 'apikey', api_key, 'output', 'eventtags');
    result = request_data(params, url);
end
