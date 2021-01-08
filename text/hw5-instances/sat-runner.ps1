# initial config
param ([int] $g, [int] $p, [int] $t, [int] $e, [double] $m, [bool] $v = 0)

# test generation size
Write-Host "Running generation experiment"
$generationCount =  20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 300, 400, 500, 600, 700, 800, 900, 1000
Foreach($i in $generationCount) {
    .\solver.ps1 -g $i -p $p -t $t -e $e -m $m -a 'Gen.exp' -v $v
}

# test population size
Write-Host "Running population experiment"
$populationCount =  20, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 150
Foreach($i in $populationCount) {
    .\solver.ps1 -g $g -p $i -t $t -e $e -m $m -a 'Pop.exp' -v $v
}

# test tournament size
Write-Host "Running tournament experiment"
$tournamentSize =  1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 20, 30, 50, 80, 100
Foreach($i in $tournamentSize) {
    .\solver.ps1 -g $g -p $p -t $i -e $e -m $m -a 'Tour.exp' -v $v
}

# mutation rate
Write-Host "Running mutation experiment"
$mutationRate =  0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1
Foreach($i in $mutationRate) {
    .\solver.ps1 -g $g -p $p -t $t -e $e -m $i -a 'Mut.exp' -v $v
}

# elitness size
Write-Host "Running elitness experiment"
$elitness =  1, 2, 3, 4, 5, 6, 7, 8, 9, 10
Foreach($i in $elitness) {
    .\solver.ps1 -g $g -p $p -t $t -e $i -m $m -a 'Elit.exp' -v $v
}

