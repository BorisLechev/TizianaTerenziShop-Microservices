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
		sh 'docker --version'
	  }
	}
	stage('Docker Build') {
	  steps {
        dir('src') {
          sh 'docker compose build'
          sh 'docker images'
        }
      }
	}
  }
}
