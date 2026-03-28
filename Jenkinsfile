pipeline {
  agent any
  stages {
    stage('Verify Branch') {
      steps {
        echo "$GIT_BRANCH"
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
          powershell(script: 'docker compose build')
          powershell(script: 'docker images')
        }
      }
	}
  }
}
