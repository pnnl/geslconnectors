% function validates API key
function result = validate_api_input(email, api_key)
    % Simple regular expression for email validation
    regex = '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$';
    
    % Email validation
    if isempty(regexpi(email, regex))
        result = 'Invalid Email';
        return;
    end

    % API key validation 
    try
        java.util.UUID.fromString(api_key); 
    catch
        result = 'Invalid API Key';
        return;
    end

    result = '';
end


