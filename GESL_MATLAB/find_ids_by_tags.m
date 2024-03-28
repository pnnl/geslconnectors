% this function returns Tag IDs
function tagsFoundStruct = find_ids_by_tags(data, tagsToFind, requiredAncestorTags)
    if nargin < 3
        requiredAncestorTags = {}; % Default to an empty cell array if not provided
    end

    tagsFound = struct();
    for i = 1:length(tagsToFind)
        tagsFound.(tagsToFind{i}) = [];
    end

    function searchStruct(structData, ancestors)
        if ~isstruct(structData)
            return;
        end

        % Handle case where structData is an array of structures
        if numel(structData) > 1
            for idx = 1:numel(structData)
                searchStruct(structData(idx), ancestors);
            end
            return;
        end

        newAncestors = ancestors;
        if isfield(structData, 'tag') && ischar(structData.tag)
            newAncestors = [newAncestors, structData.tag]; % Update ancestors
            if ismember(structData.tag, tagsToFind)
                if isempty(requiredAncestorTags) || ...
                        all(ismember(requiredAncestorTags, newAncestors))
                    id = structData.id; % Assuming 'id' is present
                    currentTag = structData.tag;
                    tagsFound.(currentTag) = [tagsFound.(currentTag), id];
                end
            end
        end

        fields = fieldnames(structData);
        for i = 1:numel(fields)
            field = fields{i};
            value = structData.(field);

            if isstruct(value) || iscell(value)
                searchStruct(value, newAncestors); % Recursive call
            end
        end
    end

    searchStruct(data, {}); % Initial call
    tagsFoundStruct = tagsFound;
end
