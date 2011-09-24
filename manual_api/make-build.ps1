# Make doxygen documentation

[Reflection.Assembly]::loadWithPartialName("system.io")


$baseDir = [system.io.path]::GetDirectoryName( $myinvocation.MyCommand.Definition )

$out = "out"
$out = [System.io.path]::combine( $baseDir , $out )

$exe = "doxygen\doxygen.exe"
$exe = [System.io.path]::combine( $baseDir , $exe )

$conf = "doxygen_framework_config.conf"
$conf = [System.io.path]::combine( $baseDir , $conf )

if ([System.io.directory]::Exists($out)){
	[System.io.directory]::Delete($out, $true) | out-null
}else{
	[System.io.directory]::CreateDirectory($out) | out-null
}
	
& $exe $conf
