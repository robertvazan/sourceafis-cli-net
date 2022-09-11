# This script generates and updates project configuration files.

# We are assuming that project-config is available in sibling directory.
# Checkout from https://github.com/robertvazan/project-config
import pathlib
project_directory = lambda: pathlib.Path(__file__).parent.parent
config_directory = lambda: project_directory().parent/'project-config'
exec((config_directory()/'src'/'net.py').read_text())

root_namespace = lambda: 'SourceAFIS.Cli'
pretty_name = lambda: 'SourceAFIS CLI for .NET'
nuget_title = lambda: 'SourceAFIS CLI'
repository_name = lambda: 'sourceafis-cli-net'
subdomain = lambda: 'sourceafis'
homepage = lambda: website() + 'cli'
inception_year = lambda: 2021
nuget_description = lambda: 'Command-line interface for SourceAFIS.'
nuget_tags = lambda: 'fingerprint; biometrics; authentication; sourceafis'
md_description = lambda: '''\
    SourceAFIS CLI for .NET is a command-line interface to [SourceAFIS for .NET](https://sourceafis.machinezoo.com/net).
    At the moment, it can benchmark algorithm accuracy, template footprint, and implementation speed.
    It also includes tools that aid in development of SourceAFIS for .NET.
    Read more on [SourceAFIS CLI](https://sourceafis.machinezoo.com/cli) page.
'''

def documentation_links():
    yield 'SourceAFIS CLI', 'https://sourceafis.machinezoo.com/cli'
    yield 'SourceAFIS for .NET', 'https://sourceafis.machinezoo.com/net'
    yield 'SourceAFIS overview', 'https://sourceafis.machinezoo.com/'

def dependencies():
    use('SourceAFIS:3.13.0')
    use('System.Net.Http:4.3.4')

generate()
