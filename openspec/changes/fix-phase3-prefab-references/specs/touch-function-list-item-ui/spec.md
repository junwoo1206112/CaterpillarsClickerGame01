# Touch Function List Item UI

## ADDED Requirements

### Requirement: Prefab UI Elements Structure
The TouchFunctionListItem prefab SHALL have all required UI elements with correct names matching the TouchFunctionListItem.cs component fields.

#### Scenario: All UI elements present
- **WHEN** the prefab is instantiated
- **THEN** the following UI elements SHALL exist:
  - NameText (Text)
  - DescriptionText (Text)
  - CostText (Text)
  - LevelText (Text)
  - PointsText (Text)
  - AddButton (Button)
  - RemoveButton (Button)

#### Scenario: UI references connected
- **WHEN** the prefab is created by Phase3Setup.CreatePrefab()
- **THEN** all UI element references in TouchFunctionListItem component SHALL be automatically connected
- **AND** no reference SHALL be null

### Requirement: UI Element Display
Each UI element SHALL display the correct data from TouchFunctionData.

#### Scenario: Function name displayed
- **WHEN** TouchFunctionListItem.Initialize() is called with TouchFunctionData
- **THEN** NameText.text SHALL equal TouchFunctionData.Name

#### Scenario: Function description displayed
- **WHEN** TouchFunctionListItem.Initialize() is called with TouchFunctionData
- **THEN** DescriptionText.text SHALL equal TouchFunctionData.Description

#### Scenario: Function cost displayed
- **WHEN** TouchFunctionListItem.Initialize() is called with TouchFunctionData
- **THEN** CostText.text SHALL equal "{TouchFunctionData.Cost} pts"

#### Scenario: Function level displayed
- **WHEN** TouchFunctionListItem.Initialize() is called with TouchFunctionData
- **THEN** LevelText.text SHALL equal "Lv. {TouchFunctionData.Level}"

#### Scenario: Current points displayed
- **WHEN** TouchFunctionListItem.Initialize() is called
- **THEN** PointsText.text SHALL equal "Points: {TouchFunctionListManager.Instance.TouchPoints}"

### Requirement: Button Interaction
The Add and Remove buttons SHALL correctly trigger function activation and deactivation.

#### Scenario: Add button triggers function activation
- **WHEN** user clicks the AddButton
- **THEN** TouchFunctionListManager.AddFunction(data.ID) SHALL be called
- **AND** AddButton SHALL become hidden
- **AND** RemoveButton SHALL become visible

#### Scenario: Remove button triggers function deactivation
- **WHEN** user clicks the RemoveButton
- **THEN** TouchFunctionListManager.RemoveFunction(data.ID) SHALL be called
- **AND** RemoveButton SHALL become hidden
- **AND** AddButton SHALL become visible

### Requirement: Button Visibility State
The buttons SHALL reflect the current activation state of the function.

#### Scenario: Inactive function shows Add button
- **WHEN** TouchFunctionListItem.SetActiveState(false) is called
- **THEN** AddButton.gameObject.activeSelf SHALL be true
- **AND** RemoveButton.gameObject.activeSelf SHALL be false

#### Scenario: Active function shows Remove button
- **WHEN** TouchFunctionListItem.SetActiveState(true) is called
- **THEN** RemoveButton.gameObject.activeSelf SHALL be true
- **AND** AddButton.gameObject.activeSelf SHALL be false