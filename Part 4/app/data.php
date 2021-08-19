<!DOCTYPE html>
<html>
<head>
    <title>Data - Plymouth Air Quality</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width-device-width, initial-scale=1">
    <link rel="stylesheet" href="resources/css/data.css" />
    <link href="https://api.mapbox.com/mapbox-gl-js/v2.4.1/mapbox-gl.css" rel="stylesheet">
    <script src="https://api.mapbox.com/mapbox-gl-js/v2.4.1/mapbox-gl.js"></script>
    <link rel="stylesheet" href="resources/css/w3schools_template.css" />
</head>

<body>

<div class="topnav">
    <a href="../app/index.php">Home</a>
    <a href="../app/data.php">Air Quality Data</a>
    <a href="../airquality/index.php">Machine Readable Data</a>
</div>


<main>
    <div class="main">

        <h1>Data Visualisation</h1>

        <div id ="data">
            <p>Below you will be able to view both a table displaying air quality data, alongside an interactive map that displays the air quality for Plymouth and the surrounding area</p>
            <p>The results were taken from local Hospitals and GP Practices</p>
            <p>The quality of air is measured in PM 2.5 - PM 2.5 refers to fine particulate matter in the air that are 2.5 microns (0.00025cm), or less, in width</p>
            <p>Particles within this size range are able to a range of health issues - more information is available on the
                <a href="https://laqm.defra.gov.uk/faqs/faq141/">laqm.defra.gov.uk</a> website</p>
            <div id="table">

                <table id="air_quality">
                    <tr>
                        <th>Name</th>
                        <th>Address</th>
                        <th>Post Code</th>
                        <th>Town</th>
                        <th>Type</th>
                        <th>PM 2.5 level</th>
                        <th>Latitude</th>
                        <th>Longitude</th>
                    </tr>
                <?php
                    $csv = array_map('str_getcsv', file("https://plymouth.thedata.place/dataset/772613d4-21ee-406e-a694-4a1dab88e268/resource/cd162ad1-d7d5-42a9-b1ab-0edbcd697f1e/download/air-quality-by-pm2.5-score-blf.org.uk.csv"));
                    array_walk($csv, function(&$a) use ($csv) {
                        $a = array_combine($csv[0], $a);
                    });
                    array_shift($csv);

                    $col = array_column($csv, "PM2_5");
                    array_multisort($col, SORT_ASC, $csv);

                    foreach($csv as $column=>$row) {
                        $i = 0;
                        echo "<tr>";
                        foreach($row as $column2=>$row2) {
                            if ($i != 6) {
                                echo "<td>" . $row2 . "</td>";
                            }
                            $i += 1;
                        }
                        echo "</tr>";
                    }
                    ?>
                </table>

            </div>
            <br>
            <h2>Interactive map</h2>
            <p>The size of the circles represents the concentration of PM 2.5 in the air, hence the larger the circles the worse the pollution in that area</p>
            <div id="map" style="width: 50vw; height: 30vw;"></div>
            <script>
                mapboxgl.accessToken = 'pk.eyJ1IjoiZXhhZ3QiLCJhIjoiY2tzaHpldjNjMWZuZDMwb2Y4dndyeGVmeiJ9.8Exo1jTGhfMRJ3QBFOSIfw';
                var map = new mapboxgl.Map({
                    container: 'map',
                    style: 'mapbox://styles/exagt/ckshyb02a58s217oc7dt38ldb'
                });
            </script>
        </div>
    </div>
</main>
</body>
</html>