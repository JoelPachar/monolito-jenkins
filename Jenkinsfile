pipeline {
    agent { label 'windows' } 
    
    stages {
        stage('Restaurar paquetes NuGet') {
            steps {
                echo 'Restaurando paquetes...'
                // Nota: Si la ruta de tu Visual Studio es diferente (ej. Enterprise en vez de Community, o 2026 en vez de 2022), ajústala aquí:
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2026\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" -t:restore "Deber_Linq.sln"'
            }
        }
        
        stage('Compilar el Monolito') {
            steps {
                echo 'Compilando el proyecto...'
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2026\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" "Deber_Linq.sln" /p:Configuration=Release'
            }
        }
        
        stage('Desplegar') {
            steps {
                echo '¡Compilación exitosa! Todo listo.'
            }
        }
    }
}