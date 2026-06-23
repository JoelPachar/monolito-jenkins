pipeline {
    agent { label 'windows' } 
    
    stages {
        stage('Restaurar paquetes NuGet') {
            steps {
                echo 'Restaurando paquetes...'
                bat '"C:\\Program Files\\Microsoft Visual Studio\\18\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" -t:restore "Deber_Linq.sln"'
            }
        }
        
        stage('Compilar el Monolito') {
            steps {
                echo 'Compilando el proyecto...'
                bat '"C:\\Program Files\\Microsoft Visual Studio\\18\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" "Deber_Linq.sln" /p:Configuration=Release'
            }
        }
        
        stage('Desplegar') {
            steps {
                echo '¡Compilación exitosa! Todo listo para entregar el deber.'
            }
        }
    }
}