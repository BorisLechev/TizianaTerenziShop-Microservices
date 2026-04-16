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
		powershell 'docker --version'
		powershell 'docker compose version'
	  }
	}
	stage('Docker Build') {
	  steps {
        dir('src') {
		  powershell 'docker compose --profile build-only build base-runtime'
          powershell 'docker compose build'
          powershell 'docker images'
        }
      }
	}
	stage('Run Application') {
      steps {
	    dir('src') {
		  powershell(script: 'docker compose up -d', returnStatus: true)
		}
      }
    }
    stage('Run Integration Tests') {
      steps {
        powershell './Tests/ContainerTests.ps1'
      }
    }
    stage('Stop Application') {
	  steps {
	    dir('src') {
		  powershell(script: 'docker compose down', returnStatus: true)
		  powershell 'docker volume prune --force'
		}
      }
      post {
	    success {
	      echo "Build successfull! You should deploy! :)"
	    }
	    failure {
	      echo "Build failed! You should receive an e-mail! :("
	    }
      }
    }
  }
}
