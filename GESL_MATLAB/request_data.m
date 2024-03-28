% this function used by other functions to request information from the
% server
function response = request_data(params, url)
    options = weboptions('ContentType', 'json', 'MediaType', 'application/json');
    options.CertificateFilename=('');
    
    try
        response = webwrite(url, params, options);
    catch e
       if isa(e, 'matlab.net.http.HTTPException')
        % Display detailed error information
        fprintf('Status Code: %s\n', e.StatusCode);
        fprintf('Status Message: %s\n', e.StatusLine);
        if ~isempty(e.Response.Body.Data)
            fprintf('Response Body: %s\n', jsonencode(e.Response.Body.Data));
        end
        else
        fprintf('Non-HTTP error: %s\n', e.message);
       end
        return; 
    end
end




