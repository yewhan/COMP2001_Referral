<?php
header("Content-Type: application/json");

$csv = fopen("../app/resource/air-quality.csv", "r");
$column = fgetcsv($csv, "1024", ",");
$array = array();
while ($row = fgetcsv($csv, "1024", ",")) {
    $array[] = array_combine($column, $row);
}

fclose($csv);

$json = json_encode($array);
echo $json;
