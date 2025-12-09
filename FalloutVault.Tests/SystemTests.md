# System Tests

<!-- TOC -->
* [System Tests](#system-tests)
  * [Test 1: Power Button](#test-1-power-button)
    * [Prerequisites](#prerequisites)
    * [Steps](#steps)
    * [Expected Result](#expected-result)
  * [Test 2: Generator Power Button](#test-2-generator-power-button)
    * [Prerequisites](#prerequisites-1)
    * [Steps](#steps-1)
    * [Expected Result](#expected-result-1)
  * [Test 3: Timed On/Off Button](#test-3-timed-onoff-button)
    * [Prerequisites](#prerequisites-2)
    * [Steps](#steps-2)
    * [Expected Result](#expected-result-2)
  * [Test 4: Slider](#test-4-slider)
    * [Prerequisites](#prerequisites-3)
    * [Steps](#steps-3)
    * [Expected Result](#expected-result-3)
  * [Test 5: Numeric Up/Down](#test-5-numeric-updown)
    * [Prerequisites](#prerequisites-4)
    * [Steps](#steps-4)
    * [Expected Result](#expected-result-4)
  * [Test 6: Dropdown Selector](#test-6-dropdown-selector)
    * [Prerequisites](#prerequisites-5)
    * [Steps](#steps-5)
    * [Expected Result](#expected-result-5)
  * [Test 7: Vent/Door Toggle](#test-7-ventdoor-toggle)
    * [Prerequisites](#prerequisites-6)
    * [Steps](#steps-6)
    * [Expected Result](#expected-result-6)
  * [Test 8: Vent/Door Lock](#test-8-ventdoor-lock)
    * [Prerequisites](#prerequisites-7)
    * [Steps](#steps-7)
    * [Expected Result](#expected-result-7)
  * [Test 9: Filter by Zone](#test-9-filter-by-zone)
    * [Prerequisites](#prerequisites-8)
    * [Steps](#steps-8)
    * [Expected Result](#expected-result-8)
  * [Test 10: Filter by Device](#test-10-filter-by-device)
    * [Prerequisites](#prerequisites-9)
    * [Steps](#steps-9)
    * [Expected Result](#expected-result-9)
  * [Test 11: Order by Zone](#test-11-order-by-zone)
    * [Prerequisites](#prerequisites-10)
    * [Steps](#steps-10)
    * [Expected Result](#expected-result-10)
  * [Test 12: Order by Device](#test-12-order-by-device)
    * [Prerequisites](#prerequisites-11)
    * [Steps](#steps-11)
    * [Expected Result](#expected-result-11)
  * [Test 13: Double-Click Log to go to Device](#test-13-double-click-log-to-go-to-device)
    * [Prerequisites](#prerequisites-12)
    * [Steps](#steps-12)
    * [Expected Result](#expected-result-12)
  * [Test 14: Search Logs](#test-14-search-logs)
    * [Prerequisites](#prerequisites-13)
    * [Steps](#steps-13)
    * [Expected Result](#expected-result-13)
  * [Test 15: Power Draw Warning](#test-15-power-draw-warning)
    * [Prerequisites](#prerequisites-14)
    * [Steps](#steps-14)
    * [Expected Result](#expected-result-14)
<!-- TOC -->

---

## Test 1: Power Button
### Prerequisites

- At least one device visible
- Generator turned on, and power available

### Steps

1. click power button

### Expected Result

1. Power button will change colour (red -> green or vice versa)
2. A log will be made indicating the change in state

---

## Test 2: Generator Power Button

### Prerequisites

- Generator is visible and turned on
- Other powered devices are on

### Steps

1. Click the power button

### Expected Result

1. The change in state  is logged
2. All other powered devices are turned off
3. All those devices' change in state is logged
4. Warnings for devices being turned off for lack of power are logged


---

## Test 3: Timed On/Off Button

### Prerequisites

- A Light device or Fan device is visible
- Generator is turned on, and power available

### Steps

1. Click timed on/off button
2. Click the time selection in the dialog window
2. Select 10 seconds
3. Click the check mark
4. Click accept

### Expected Result

1. The change in state is logged as the device is turned on or off
2. The button shows a countdown timer for 10 seconds
3. After 10 seconds passes, the device changes back to the original state
4. The change in state is logged

---

## Test 4: Slider

### Prerequisites

- A device that inplements a slider is visible

### Steps

1. Adjust the slider

### Expected Result

1. The text above the slider changes to show the current value
2. Each new value is logged in the log viewer

---

## Test 5: Numeric Up/Down

### Prerequisites

- A Fan device is visible
- The device is powered on
- The generator is on, and there is power available

### Steps

1. Click and hold the up arrow until the number stops increasing
2. Wait for the Current speed to stop increasing
2. Click into the box and type the number zero

### Expected Result

1. The target speed will cap at a value set by the controller
2. A log is made showing the change
2. The current speed will linearly approach the target and hover near that value
3. The target speed will be set back to zero
4. A log is made showing the change
4. The current speed will linearly decrease until it reaches zero and stops changing

---

## Test 6: Dropdown Selector

### Prerequisites

- A sprinkler device is visible
- The generator is on, and power is available

### Steps

1. Click the target section drop down
2. Select new value

### Expected Result

1. The new target will be shown in the dropdown button
2. A log is made showing the change

---

## Test 7: Vent/Door Toggle

### Prerequisites

- At least 1 vent seal controller is visible in the devices list and unlocked.
- At least 1 door controller is visible in the devices list and unlocked.

### Steps

1. Repeat the following steps for 1 vent and 1 door:
   1. Open the device.
   2. Close the device.

### Expected Result

- True for each device:
  1. The device was successfully opened (green icon).
  2. The device was successfully closed (red icon).

---

## Test 8: Vent/Door Lock

### Prerequisites

- At least 1 vent seal controller is visible in the devices list.
- At least 1 door controller is visible in the devices list.

### Steps

1. Repeat the following steps for 1 vent and 1 door:
    1. Lock the device.
    2. Unlock the device.

### Expected Result

- True for each device:
    1. The device was successfully locked.
    2. The device was successfully unlocked.

---

## Test 9: Filter by Zone

### Prerequisites

- All zones are enabled in _Filter by Zone_.
- All devices are enabled in _Filter by Device_.

### Steps

1. Uncheck all zones either manually or using the checkbox in the column header.
2. Enable _Entrance_ and _Generator Room_ zone checkboxes.

### Expected Result

Only devices from the _Entrance_ and _Generator Room_ zones are present in the devices list.

---

## Test 10: Filter by Device
### Prerequisites

- All zones are enabled in _Filter by Zone_.
- All devices are enabled in _Filter by Device_.

### Steps

1. Uncheck all devices either manually or using the checkbox in the column header.
2. Enable _Power Controller_ and _Fan Controller_ device checkboxes.

### Expected Result

Only _Power Controller_ and _Fan Controller_ devices are present in the devices list.

---

## Test 11: Order by Zone

### Prerequisites

- All zones are enabled in _Filter by Zone_.
- All devices are enabled in _Filter by Device_.

### Steps

1. Click `Zone` button in _Order By_ section.

### Expected Result

The devices list is now sorted by zones in the same order as _Filter by Zone_.

---

## Test 12: Order by Device

### Prerequisites

- All zones are enabled in _Filter by Zone_.
- All devices are enabled in _Filter by Device_.

### Steps

1. Click `Device` button in _Order By_ section.

### Expected Result

The devices list is now sorted by device types in the same order as _Filter by Device_.

---

## Test 13: Double-Click Log to go to Device

### Prerequisites

- All zones are enabled in _Filter by Zone_.
- All devices are enabled in _Filter by Device_.
- At least one log entry is present in the log viewer.

### Steps

1. Double-click the log entry.

### Expected Result

The device that emitted the log is visible in the devices list.

---

## Test 14: Search Logs

### Prerequisites

- The entrance light controller has been turned on and off at least once.
- The entrance speaker controller has been turned on and off at least once.

### Steps

1. Search for `entrance`
2. Search for `light`
3. Search for `speaker`
4. Search for `on`
5. Search for `off`
6. Search for `true`
7. Search for `false`

### Expected Result

1. Only the logs from entrance devices are present.
2. Only the logs from lights are present.
3. Only the logs from speakers are present.
4. Only logs containing "on" in the sender, message, or value are present.
5. Only logs containing "off" in the sender, message, or value are present.
6. Only logs containing "true" in the sender, message, or value are present.
7. Only logs containing "false" in the sender, message, or value are present.

---

## Test 15: Power Draw Warning

### Prerequisites

- `Power Draw` is less than `Power Available`

### Steps

1. Turn on all devices until `Power Draw` is 80-90% of `Power Generated`.
   1. Turn on all lights at maximum brightness.
   2. Turn on all fans at maximum target RPM.
   3. Turn on all speakers at maximum volume.
   4. Turn on all crop sprinklers.

### Expected Result

The message `Warning: High Power Consumption` is visible in the pinned power bar at the top of the interface.