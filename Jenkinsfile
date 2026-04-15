pipeline {
  agent any
  stages {
    stage('Verify Branch') {
      steps {
        echo "$GIT_BRANCH"
      }
    }
	stage('Run Unit Tests') {
	  when {
		expression { fileExists('src/TizianaTerenzi.sln') }
	  }
	  steps {
		dir('src') {
		  powershell 'dotnet test TizianaTerenzi.sln'
		}
	  }
	}
	stage('Check Docker') {
	  steps {
		powershell(script: 'docker --version')
		powershell(script: 'docker compose version')
	  }
	}
	stage('Docker Build') {
	  steps {
        dir('src') {
		  powershell(script: 'docker compose --profile build-only build base-runtime')
          powershell(script: 'docker compose build')
          powershell(script: 'docker images')
        }
      }
	}
  }
}
